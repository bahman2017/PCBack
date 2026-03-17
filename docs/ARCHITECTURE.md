# Backend Architecture

PatentClarity backend follows a simple modular architecture suitable for MVP development.

Architecture flow:

Controller → Service Layer → AI Service

Responsibilities:

Controller
Handles HTTP requests and responses.

Service Layer
Contains business logic and orchestrates external services.

AI Service
Handles AI-based analysis and report generation.

Folder Structure:

PCBack
│
├── Controllers
│     PatentsController.cs
│
├── Services
│     PatentService.cs
│     AiAnalysisService.cs
│
├── Models
│     PatentAnalysisRequest.cs
│     PatentMetadata.cs
│     CommercialReport.cs
│
└── docs

Future architecture improvements may include:

• PostgreSQL persistence
• AI prompt orchestration layer
• patent ingestion pipeline
• semantic patent search
