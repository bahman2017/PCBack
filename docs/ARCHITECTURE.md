# Backend Architecture

PatentClarity backend uses a layered structure suitable for MVP and growth.

## Request flow

```
HTTP → PatentsController
         → IPatentService (PatentService)     [PatentSearch API]
         → IAiAnalysisService (AiAnalysisService)
              → PromptBuilder
              → IAiClient (AiClient)          [OpenAI]
              → JSON → CommercialReport
```

## Responsibilities

| Layer | Responsibility |
|--------|----------------|
| **Controllers** | HTTP, validation, composing metadata + report |
| **Services** | Patent fetch, AI orchestration |
| **AI** | Prompts, LLM HTTP client, structured result DTOs |
| **Models** | API contracts (`PatentAnalysisRequest`, `CommercialReport`, `PatentMetadata`) |

## Folder structure (source: `src/PCBack/`)

```
PCBack/
├── Controllers/
│   └── PatentsController.cs
├── Services/
│   ├── IPatentService.cs
│   ├── PatentService.cs
│   ├── PatentsViewDto.cs          (internal PatentSearch request/response)
│   ├── IAiAnalysisService.cs
│   └── AiAnalysisService.cs
├── AI/
│   ├── PromptTemplates.cs
│   ├── PromptBuilder.cs
│   ├── AiClient.cs
│   ├── OpenAiDto.cs
│   └── AiAnalysisResult.cs
├── Models/
│   ├── PatentAnalysisRequest.cs
│   ├── PatentMetadata.cs
│   └── CommercialReport.cs
├── Program.cs
└── Properties/launchSettings.json   (e.g. http://localhost:5000)
```

Repository root also contains `docs/` for project documentation.

## External systems

- **Patent metadata:** `https://search.patentsview.org/api/v1/patents` (POST, JSON body with `filter` / `fields`)
- **LLM:** `https://api.openai.com/` (chat completions)

## Future architecture

- PostgreSQL persistence and caching
- Stronger prompt/versioning and evaluation
- Patent ingestion and semantic search
- Auth, billing, rate limits
