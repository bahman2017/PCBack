# Backend Architecture

PatentClarity backend uses a layered structure suitable for MVP and growth.

## Request flow (analyze)

```
HTTP → PatentsController
         → IPatentService (PatentService)     [PatentSearch API]
         → IAiAnalysisService (AiAnalysisService)
              → PromptBuilder
              → IAiClient (AiClient)          [OpenAI]
              → JSON → CommercialReport
         → (optional) EF Core → patent_analyses
```

## Request flow (report by id / PDF / checkout)

```
GET  /api/patents/{id}     → IReportService.GetByIdAsync → CommercialReport
GET  /api/reports/{id}/pdf → IReportService + IPdfService (QuestPDF) → PDF bytes
POST /api/payments/checkout → IPaymentService → IReportService (existence) → checkout URL
```

## Responsibilities

| Layer | Responsibility |
|--------|----------------|
| **Controllers** | HTTP, validation, composing metadata + report; persistence on analyze |
| **Services** | Patent fetch, AI orchestration, report load, PDF build, payment session (mock / future Stripe) |
| **AI** | Prompts, LLM HTTP client, structured result DTOs |
| **Data** | EF Core `ApplicationDbContext`, `PatentAnalysis` entity |
| **Models** | API contracts (`PatentAnalysisRequest`, `CommercialReport`, `PatentMetadata`, payment DTOs, `PaymentMode`) |

## Folder structure (source: `src/PCBack/`)

```
PCBack/
├── Controllers/
│   ├── PatentsController.cs
│   ├── ReportsController.cs
│   └── PaymentsController.cs
├── Services/
│   ├── IPatentService.cs, PatentService.cs, PatentsViewDto.cs
│   ├── IAiAnalysisService.cs, AiAnalysisService.cs
│   ├── IReportService.cs, ReportService.cs
│   ├── IPdfService.cs, PdfService.cs
│   ├── IPaymentService.cs, PaymentService.cs
│   └── (PatentSearch HTTP client registration in Program.cs)
├── Data/
│   └── ApplicationDbContext.cs
├── Migrations/
├── AI/
│   ├── PromptTemplates.cs
│   ├── PromptBuilder.cs
│   ├── AiClient.cs
│   ├── OpenAiDto.cs
│   └── AiAnalysisResult.cs
├── Models/
│   ├── PatentAnalysis.cs, PatentAnalysisRequest.cs, PatentAnalysisHistoryItem.cs
│   ├── PatentMetadata.cs, CommercialReport.cs
│   ├── PaymentMode.cs, PaymentOptions.cs
│   └── PaymentCheckoutRequest.cs, PaymentCheckoutResponse.cs
├── Program.cs, Program.Markers.cs
└── Properties/launchSettings.json   (e.g. http://localhost:5000)
```

Repository root also contains `docs/` for project documentation.

## External systems

- **Patent metadata:** `https://search.patentsview.org/api/v1/patents` (POST, JSON body with `filter` / `fields`)
- **LLM:** `https://api.openai.com/` (chat completions)
- **Payments (MVP):** no external PSP; mock URL only. Future: Stripe (or similar) behind `PaymentMode`.

## Future architecture

- Stronger prompt/versioning and evaluation
- Patent ingestion and semantic search
- Auth, billing, rate limits (beyond mock checkout)
- Caching for metadata and LLM responses
