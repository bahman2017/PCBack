# Patent Data Sources

Project: PatentClarity  
Backend: PCBack  
Last updated: March 24, 2026

---

# Purpose

PatentClarity needs reliable sources for **title, abstract, assignee, filing/grant date**, and derived **status** (e.g. Active vs Expired). This document lists APIs and datasets relevant to the backend.

---

# 1. PatentsView PatentSearch API (current MVP integration)

**Website / docs:** [patentsview.org](https://patentsview.org) — transition and Search API references.

**Endpoint used in PCBack:**

`POST https://search.patentsview.org/api/v1/patents`

**Request shape (simplified):**

```json
{
  "filter": { "patent_number": ["10000000"] },
  "fields": [
    "patent_number",
    "patent_title",
    "patent_abstract",
    "assignee_organization",
    "patent_date"
  ]
}
```

**Response:** JSON with a `data` array of patent objects (including `assignees` with `assignee_organization`).

**Implementation:** `PatentService` + internal DTOs in `Services/PatentsViewDto.cs`.

**Notes:**

- The **legacy** `https://api.patentsview.org/patents/query` endpoint is discontinued; PCBack uses the Search API above.
- PatentsView continues to evolve (e.g. USPTO Open Data Portal migration); monitor official notices.

---

# 2. Google Patents

**Website:** https://patents.google.com

Large global index; **no official public API** for the same use case. Possible future: partner APIs or licensed feeds.

---

# 3. USPTO Open Data Portal

**Website:** https://developer.uspto.gov / https://data.uspto.gov

Official bulk and API-oriented data. Strong for **batch** and compliance; not always the fastest path for a single-number lookup compared to a search API.

---

# 4. The Lens

**Website:** https://www.lens.org

Global patents + scholarly links. API may require registration and has usage limits. Good for **research** and cross-domain discovery.

---

# Recommended strategy for MVP

- **Primary:** PatentSearch (`search.patentsview.org`) for US-style patent number lookups, as implemented.
- **Later:** Combine Lens, USPTO bulk, or others for global coverage and analytics.

---

# Future data strategy

- Multi-source normalization (one internal `PatentMetadata` model)  
- Caching and persistence to reduce API cost and rate-limit risk  
- Semantic search and citation graphs (see roadmap)
