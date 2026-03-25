# Development Log

## 2026-03-17

Initial backend MVP created.

Implemented:

- ASP.NET Core Web API project
- Patent analysis endpoint
- Service layer architecture
- PatentService placeholder
- AiAnalysisService placeholder
- Input validation

Endpoint: POST /api/patents/analyze

Next: Integrate real patent metadata.

---

## 2026-03-24

Patent metadata and AI pipeline brought to production shape.

Implemented:

- **PatentSearch API** integration (`search.patentsview.org/api/v1/patents`) replacing legacy PatentsView query endpoint
- Internal DTOs (`PatentsViewDto.cs`) and `PatentService` with `HttpClient`
- **AI folder:** `PromptTemplates`, `PromptBuilder`, `AiAnalysisResult`, `OpenAiDto`
- **JSON-only** LLM output contract and parsing in `AiAnalysisService`
- **OpenAI** `gpt-4o-mini` via `AiClient` (`AddHttpClient<IAiClient, AiClient>`)
- `OpenAI:ApiKey` configuration; graceful empty response when key missing or call fails
- `HttpClient` for patent service: `UseCookies = false` on primary handler
- Launch settings: fixed ports (e.g. HTTP 5000)

Documentation updated under `docs/` to match current architecture.

Next: PostgreSQL / EF Core, caching, users, usage tracking.
