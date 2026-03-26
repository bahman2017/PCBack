# PatentClarity – Project Status

Last Updated: 2026-03-25

---

# Current Phase

MVP Backend – AI patent analysis with **persistence** and **automated tests**

The backend supports PatentSearch metadata, OpenAI-backed commercialization output (when configured), **PostgreSQL storage** of each analysis, and a **history** API.

---

# Completed Components

## 1. Backend API (ASP.NET Core)

Project: PCBack (`src/PCBack/`)

| Endpoint | Method | Purpose |
|----------|--------|---------|
| `/api/patents/analyze` | POST | Patent number and/or abstract → `CommercialReport` |
| `/api/patents/history` | GET | Last **20** persisted analyses (newest first) |

**POST** input (`PatentAnalysisRequest`): optional `patentNumber`, optional `abstract` (at least one required).

**POST** output: `CommercialReport` (unchanged contract).

After a successful report build, the controller **persists** a row to `patent_analyses`. If the database is unavailable, the API still returns **200** with the report and logs a warning.

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

## 6. Application wiring (`Program.cs`)

- `AddDbContext`: **Npgsql** (default + Development) or **InMemory** (**Testing** only)
- `AddControllers`, `PromptBuilder`, `HttpClient` for `IAiClient` and `IPatentService`
- **`Program.Markers.cs`:** exposes `partial Program` for `WebApplicationFactory<Program>`
- HTTPS redirection **skipped** in **Testing** (integration tests use HTTP client)

---

# Current System Flow

Patent input → (optional) PatentSearch metadata → `AiAnalysisService` → report → controller merges metadata → **persist** `PatentAnalysis` → JSON response

History: **GET** reads latest rows from the same database.

---

# Automated tests (`PCBack.Tests/`)

| Area | Notes |
|------|--------|
| **Unit-style** | `Fakes/AiAnalysisServiceFake` + `Tests/CacheTests.cs` (InMemory, cache hit/miss patterns) |
| **Integration** | `TestInfrastructure/CustomWebApplicationFactory.cs` + `Fakes/FakeAiAnalysisService.cs` + `Tests/IntegrationTests.cs` — real HTTP pipeline, InMemory DB, **no OpenAI** |

Run: `dotnet test` from repo root.

---

# Current Limitations (Next)

- No cross-request **cache** in production API (only test fakes model “cache”)
- No user accounts, auth, or usage metering
- No distributed cache / read replicas

---

# Next Development Phase

- Result / metadata **caching** (Redis or DB-backed)
- Auth, rate limits, billing
- Stronger prompt evaluation and monitoring

---

# Project Health

Backend: stable  
Persistence: PostgreSQL in non-Testing environments  
AI: operational when `OpenAI:ApiKey` is set  
Tests: **xUnit** — unit + `WebApplicationFactory` integration  

Next focus: caching, cost controls, SaaS hardening.
