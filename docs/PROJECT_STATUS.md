# PatentClarity – Project Status

Last Updated: 2026-04-04

---

# Current Phase

MVP Backend – AI patent analysis with **persistence**, **report retrieval**, **PDF export**, **mock checkout**, and **automated tests**

The backend supports PatentSearch metadata, OpenAI-backed commercialization output (when configured), **PostgreSQL** storage of each analysis, **history** and **by-id** APIs, **PDF** generation for a saved report, and a **payments checkout** endpoint that returns a placeholder URL (no real Stripe yet).

---

# Completed Components

## 1. Backend API (ASP.NET Core)

Project: PCBack (`src/PCBack/`)

| Endpoint | Method | Purpose |
|----------|--------|---------|
| `/api/patents/analyze` | POST | Patent number and/or abstract → `CommercialReport` (persisted when DB available) |
| `/api/patents/history` | GET | Last **20** persisted analyses (newest first) |
| `/api/patents/{id}` | GET | Single persisted report by id (`CommercialReport` shape); **404** if missing |
| `/api/reports/{id}/pdf` | GET | PDF download for persisted report; **404** if missing |
| `/api/payments/checkout` | POST | Body `{ "reportId": "<guid>" }` → `{ "checkoutUrl": "<url>" }` (mock); **404** if report missing |

**POST** `/api/patents/analyze` input (`PatentAnalysisRequest`): optional `patentNumber`, optional `abstract` (at least one required).

**POST** output: `CommercialReport` (unchanged contract). The persisted row’s **id** is not returned on this response; clients can use **`GET /api/patents/history`** (`id` on each item) or poll after UX flows that store ids server-side.

After a successful report build, the controller **persists** a row to `patent_analyses`. If the database is unavailable, the API still returns **200** with the report and logs a warning.

**POST** `/api/payments/checkout`: **400** if body missing or `reportId` empty; **501** if `Payment:Mode` is **Stripe** (not implemented). Default mode is **Mock** (`appsettings.json` → `Payment:Mode`).

---

## 2. Patent Metadata Retrieval

Integrated with **PatentsView PatentSearch API**.

- **Endpoint:** `POST https://search.patentsview.org/api/v1/patents`
- **Code:** `Services/PatentService.cs`, `Services/PatentsViewDto.cs`
- **HTTP:** `AddHttpClient<IPatentService, PatentService>()` with `UseCookies = false` on the primary handler

Mapped to `PatentMetadata`: title, abstract, assignee, date, **Active / Expired** (20-year rule).

---

## 3. Data Layer (EF Core + PostgreSQL)

| Item | Details |
|------|---------|
| **DbContext** | `Data/ApplicationDbContext.cs` |
| **Entity** | `Models/PatentAnalysis.cs` → table `patent_analyses` |
| **Lists in DB** | `TechnologyTags`, `PotentialMarkets`, `CommercialOpportunities` stored comma-separated (MVP) |
| **Migrations** | `src/PCBack/Migrations/` (e.g. `InitialCreate`) |
| **Connection** | `ConnectionStrings:DefaultConnection` in `appsettings.json`; **Development** may override locally |

**Testing / integration tests:** when `ASPNETCORE_ENVIRONMENT` is **Testing**, `Program.cs` registers **EF InMemory** only (no Npgsql) so `WebApplicationFactory` tests do not conflict with PostgreSQL. Production and local dev (non-Testing) use **Npgsql**.

---

## 4. AI Prompt Pipeline

Under `src/PCBack/AI/`: `PromptTemplates`, `PromptBuilder`, `AiClient`, `OpenAiDto`, `AiAnalysisResult`.

Orchestration: `Services/AiAnalysisService.cs` → JSON-only LLM contract → `CommercialReport` list fields.

---

## 5. OpenAI Integration

`AI/AiClient.cs` → `gpt-4o-mini`, `OpenAI:ApiKey`. Missing key or failed call → empty model text → empty parsed lists where applicable.

---

## 6. Reports, PDF, and payments (service layer)

| Service | Role |
|---------|------|
| **`IReportService` / `ReportService`** | Load persisted `CommercialReport` by analysis id (used by patents GET, reports PDF, payments) |
| **`IPdfService` / `PdfService`** | Build PDF bytes from `CommercialReport` (QuestPDF) |
| **`IPaymentService` / `PaymentService`** | Validate report exists; **`PaymentMode.Mock`** → fake checkout URL; **`Stripe`** → not implemented |

Configuration: **`Payment`** section (`Models/PaymentOptions.cs`), enum **`PaymentMode`**: `Mock` | `Stripe`. Registered via `Configure<PaymentOptions>(...)` in `Program.cs`.

---

## 7. Application wiring (`Program.cs`)

- `Configure<PaymentOptions>` from configuration section **`Payment`**
- `AddDbContext`: **Npgsql** (default + Development) or **InMemory** (**Testing** only)
- `AddControllers`, `PromptBuilder`, `HttpClient` for `IAiClient` and `IPatentService`
- Scoped: `IAiAnalysisService`, `IReportService`, `IPaymentService`; singleton: `IPdfService`
- **`Program.Markers.cs`:** exposes `partial Program` for `WebApplicationFactory<Program>`
- HTTPS redirection **skipped** in **Testing** (integration tests use HTTP client)

---

# Current System Flow

Patent input → (optional) PatentSearch metadata → `AiAnalysisService` → report → controller merges metadata → **persist** `PatentAnalysis` → JSON response

History: **GET** reads latest rows from the same database.

By id: **GET** `/api/patents/{id}` maps DB row → `CommercialReport`.

PDF: **GET** `/api/reports/{id}/pdf` loads report → `PdfService` → `application/pdf`.

Checkout: **POST** `/api/payments/checkout` ensures report exists → returns mock URL when mode is **Mock**.

---

# Automated tests (`PCBack.Tests/`)

| Area | Notes |
|------|-------|
| **Unit-style** | `Fakes/AiAnalysisServiceFake` + `Tests/CacheTests.cs` (InMemory, cache hit/miss patterns) |
| **Integration** | `TestInfrastructure/CustomWebApplicationFactory.cs` + `Fakes/FakeAiAnalysisService.cs` + `Tests/IntegrationTests.cs` — real HTTP pipeline, InMemory DB, **no OpenAI** |

Run: `dotnet test` from repo root.

---

# Current Limitations (Next)

- No cross-request **cache** in production API (only test fakes model “cache”)
- No user accounts, auth, or usage metering
- **Payments:** mock URL only; **Stripe** not wired (`Payment:Mode` → **501** if set to Stripe)
- **POST** `/api/patents/analyze` does not echo the new row’s **id** in the JSON body
- No distributed cache / read replicas

---

# Next Development Phase

- **Stripe** (or other provider) behind `PaymentMode.Stripe`
- Result / metadata **caching** (Redis or DB-backed)
- Auth, rate limits, billing
- Optional: include persisted **id** on analyze response or redirect client to created resource
- Stronger prompt evaluation and monitoring

---

# Project Health

Backend: stable  
Persistence: PostgreSQL in non-Testing environments  
Reports / PDF / mock checkout: implemented  
AI: operational when `OpenAI:ApiKey` is set  
Tests: **xUnit** — unit + `WebApplicationFactory` integration  

Next focus: real payments, caching, cost controls, SaaS hardening.
