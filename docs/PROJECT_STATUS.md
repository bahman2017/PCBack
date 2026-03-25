# PatentClarity – Project Status

Last Updated: 2026-03-24

---

# Current Phase

MVP Backend – Core AI Patent Analysis Engine

The backend supports real patent metadata retrieval (PatentSearch API) and AI-driven commercialization analysis (OpenAI).

---

# Completed Components

## 1. Backend API (ASP.NET Core)

Project: PCBack

Main endpoint:

POST /api/patents/analyze

Input:

- PatentNumber (optional)
- Abstract (optional; at least one required)

Output:

Commercialization analysis report (`CommercialReport`).

---

## 2. Patent Metadata Retrieval

Integrated with **PatentsView PatentSearch API** (not the discontinued legacy `api.patentsview.org/patents/query` endpoint).

- **Endpoint used:** `POST https://search.patentsview.org/api/v1/patents`
- **Implementation:** `Services/PatentService.cs` with internal DTOs in `Services/PatentsViewDto.cs`
- **HTTP:** `HttpClient` registered via `AddHttpClient<IPatentService, PatentService>()`; primary handler uses `UseCookies = false` to avoid host-environment issues with `CookieContainer`.

Retrieved fields (mapped to `PatentMetadata`):

- Patent title
- Abstract
- Owner / assignee (first `assignee_organization`)
- Patent date
- Status: **Active** or **Expired** (20-year rule from `patent_date`)

Flow:

User input → PatentSearch API → metadata extraction → optional merge into report in controller

---

## 3. AI Prompt Pipeline

Components under `src/PCBack/AI/`:

| File | Role |
|------|------|
| `PromptTemplates.cs` | Base commercialization prompt; instructs JSON-only output |
| `PromptBuilder.cs` | Builds prompt from `PatentMetadata` (title + abstract) |
| `AiClient.cs` | `IAiClient` – OpenAI chat completions |
| `OpenAiDto.cs` | Request/response DTOs for OpenAI |
| `AiAnalysisResult.cs` | Parsed LLM JSON (technologyTags, potentialMarkets, commercialOpportunities) |

Orchestration: `Services/AiAnalysisService.cs` (implements `IAiAnalysisService`).

Structured LLM output:

- `technologyTags`
- `potentialMarkets`
- `commercialOpportunities`

The model is instructed to return **valid JSON only** (no prose outside the JSON object).

---

## 4. JSON Parsing System

Flow:

LLM text → extract `{ ... }` block (regex) → `JsonSerializer.Deserialize<AiAnalysisResult>` → map to `CommercialReport`

If parsing fails or the response is empty:

- `TechnologyTags`, `PotentialMarkets`, `CommercialOpportunities` end up empty (or empty after failed deserialize).
- The API does not crash.

---

## 5. OpenAI Integration

File: `AI/AiClient.cs`

- **POST** `https://api.openai.com/v1/chat/completions` (relative path `v1/chat/completions` on `HttpClient` base address)
- **Model:** `gpt-4o-mini`
- **Configuration:** `OpenAI:ApiKey` in configuration

Recommended:

- Environment variable `OpenAI__ApiKey`, or
- User Secrets (do not commit keys)

If the API key is missing or the request fails:

- `GenerateAsync` returns an empty string; downstream parsing yields empty lists where applicable.

---

## 6. Dependency Injection (`Program.cs`)

- `AddHttpClient<IPatentService, PatentService>()` + `SocketsHttpHandler { UseCookies = false }`
- `AddHttpClient<IAiClient, AiClient>()` with `BaseAddress` `https://api.openai.com/`
- `AddSingleton<PromptBuilder>()`
- `AddScoped<IAiAnalysisService, AiAnalysisService>()`

---

# Current System Flow

Patent number / abstract → PatentSearch metadata (if number) → `PromptBuilder` → OpenAI → JSON parse → `CommercialReport` (controller fills title/owner/status from metadata when present)

---

# Current Limitations (To Be Implemented)

- No database persistence
- No result caching
- No user accounts or usage tracking

---

# Next Development Phase

**Phase: Data persistence**

1. PostgreSQL + EF Core
2. `PatentAnalysis` / result tables
3. Metadata and analysis history caching

---

# Project Health

Backend: stable  
AI integration: operational when `OpenAI:ApiKey` is set  
Build: successful

Next focus: persistence and cost optimization.
