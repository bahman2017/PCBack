# PatentClarity MVP Roadmap

Last updated: April 4, 2026

This document describes the development roadmap for the PatentClarity platform.

The goal is to move from a simple MVP to a scalable SaaS platform for patent commercialization intelligence.

---

# Phase 1 — MVP (Current Phase)

Goal:
Validate that users are willing to pay for automated patent commercialization insights.

Features:

• Patent input (patent number or abstract)  
• Patent metadata retrieval  
• AI-generated commercialization report  
• Market opportunity suggestions  

Architecture:

Controller → Service Layer → AI Service

Primary endpoint:

POST /api/patents/analyze

Current status:

- Backend API and POST /api/patents/analyze operational
- Real **PatentSearch** metadata integration (`search.patentsview.org`)
- **OpenAI**-backed analysis (`gpt-4o-mini`) with JSON structured output and safe parse fallbacks
- **PostgreSQL + EF Core** persistence, **GET** history and **GET** report by id
- **PDF** export (**GET** `/api/reports/{id}/pdf`)
- **Mock** payment checkout (**POST** `/api/payments/checkout`) — placeholder URL; **Stripe** enum reserved, not implemented

Next tasks:

1. Real **payment provider** (e.g. Stripe) when `PaymentMode.Stripe` is selected
2. Improve commercialization prompts (versioning, evals)
3. Add structured logging, metrics, and optional caching for patent metadata / LLM responses
4. Rate limiting and cost controls
5. Optional: return persisted report **id** from analyze (or `201` + `Location`) for simpler client checkout flows

Success metric:

First real users generating reports.

---

# Phase 2 — Early SaaS

Goal:
Turn the prototype into a usable SaaS product.

New features:

• PostgreSQL database *(done for analyses)*  
• User accounts  
• Authentication  
• Report history *(done — list + by id)*  
• PDF export of reports *(done)*  
• Payment integration *(mock checkout done; live billing TBD)*  

Architecture additions:

• Database persistence *(in place)*  
• Report storage *(in place)*  
• User management  
• Live PSP webhooks and entitlement rules

Success metric:

First paying customers.

---

# Phase 3 — Patent Discovery Engine

Goal:
Transform PatentClarity into a patent discovery platform.

New features:

• Problem-driven patent search
• Semantic patent search
• Patent opportunity scoring
• Competitor analysis

New components:

• Patent search engine
• Opportunity scoring engine
• Vector embeddings for patent similarity

Success metric:

Users using the system to discover startup ideas.

---

# Phase 4 — Innovation Intelligence Platform

Goal:
Become a global innovation discovery platform.

Possible features:

• Startup idea generation from patents
• Licensing marketplace
• Corporate innovation scouting
• Patent investment intelligence

Success metric:

Enterprise customers and large-scale patent analytics.

---

# Core Competitive Advantage

PatentClarity does not only summarize patents.

It transforms patents into:

• startup opportunities
• licensing strategies
• product ideas
• market insights

The platform converts technical patent documents into actionable business intelligence.

---


