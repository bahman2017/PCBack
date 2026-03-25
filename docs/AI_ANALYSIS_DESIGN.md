# AI Analysis Engine Design

Project: PatentClarity  
Backend: PCBack  
Last updated: March 24, 2026

---

# Purpose

The AI analysis layer turns patent text into **actionable business insight**: technology categories, markets, startup-style opportunities, and licensing angles—not a plain summary.

---

# Input

The pipeline uses **`PatentMetadata`** (from `PatentService` or built from abstract-only input):

- Title  
- Abstract  
- PatentOwner, PatentStatus (optional for prompting; controller still applies metadata to the final report)

Future inputs may include full text, classifications, citations, inventors.

---

# Output

**`CommercialReport`** (API-facing):

- Title, PatentOwner, PatentStatus (often filled from patent metadata in the controller)  
- TechnologyTags, PotentialMarkets, CommercialOpportunities (from LLM JSON → `AiAnalysisResult`)

---

# Implemented pipeline

1. **Preparation**  
   `AiAnalysisService` builds a `PatentMetadata` with at least `Abstract` (and title when available via future extension).

2. **Prompt construction**  
   `PromptBuilder` uses `PromptTemplates.CommercializationAnalysis` with `{title}` and `{abstract}` replaced. The template requires **strict JSON** with:

   - `technologyTags`  
   - `potentialMarkets`  
   - `commercialOpportunities`  

3. **Model execution**  
   `IAiClient` / `AiClient`: POST OpenAI `v1/chat/completions`, model `gpt-4o-mini`, temperature `0.2`. Config: `OpenAI:ApiKey`.

4. **Parsing**  
   Extract JSON object from text (handles minor wrapping), deserialize to **`AiAnalysisResult`**, map lists into **`CommercialReport`**. On failure: empty lists; no crash.

---

# Code layout

| Location | Role |
|----------|------|
| `AI/PromptTemplates.cs` | System + user instructions, JSON schema |
| `AI/PromptBuilder.cs` | `Build(PatentMetadata) → string` |
| `AI/AiClient.cs` | `GenerateAsync(prompt) → string` |
| `AI/OpenAiDto.cs` | OpenAI request/response shapes |
| `AI/AiAnalysisResult.cs` | Parsed LLM JSON |
| `Services/AiAnalysisService.cs` | Orchestration + parse |

Controllers depend only on **`IAiAnalysisService`**; swapping prompts or LLM client does not change the HTTP contract.

---

# Future improvements

- Classification / embeddings, market sizing, competitor signals  
- Structured output via OpenAI JSON mode or tool calls  
- Prompt versioning, evals, and fallbacks  
- Caching and persistence (see roadmap)

---

# Design principle

Keep the AI stack **modular**: `IAiClient` and `PromptBuilder` are replaceable without changing `PatentsController`.
