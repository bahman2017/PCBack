# PatentClarity MVP Roadmap

Date: March 17, 2026

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

- Backend API created
- Placeholder patent metadata service
- Placeholder AI analysis service

Next tasks:

1. Integrate real patent metadata API (PatentsView)
2. Replace placeholder AI service with LLM analysis
3. Improve commercialization prompts
4. Add basic logging

Success metric:

First real users generating reports.

---

# Phase 2 — Early SaaS

Goal:
Turn the prototype into a usable SaaS product.

New features:

• PostgreSQL database
• User accounts
• Authentication
• Report history
• PDF export of reports
• Payment integration

Architecture additions:

• Database persistence
• Report storage
• User management

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


