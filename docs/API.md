# API Documentation

Base URL (local dev): `http://localhost:5000` (see `Properties/launchSettings.json`).

JSON responses use **camelCase** property names by default (ASP.NET Core).

---

## POST /api/patents/analyze

Analyzes a patent (by number and/or abstract) and returns a commercialization-style report.

### Request body

`PatentAnalysisRequest`

| Field | Type | Required |
|--------|------|----------|
| `patentNumber` | string | No (one of patent number or abstract) |
| `abstract` | string | No |

**Validation:** HTTP **400** if both `patentNumber` and `abstract` are missing or whitespace-only.

**Behavior:**

1. If `patentNumber` is set: `PatentService.GetPatentMetadataAsync` calls PatentSearch; abstract for analysis comes from metadata when present, otherwise from the request `abstract`.
2. If only `abstract` is set: that text is used for the AI pipeline.
3. `AiAnalysisService.GenerateCommercialReportAsync` runs the prompt → OpenAI (when configured) → JSON parse → `CommercialReport`.
4. When metadata exists, the controller overwrites `title`, `patentOwner`, and `patentStatus` on the report from metadata.

### Example requests

```json
{
  "patentNumber": "10000000"
}
```

```json
{
  "abstract": "A new method for improving lithium battery efficiency."
}
```

### Response

`CommercialReport`

| Field | Type |
|--------|------|
| `title` | string |
| `patentOwner` | string |
| `patentStatus` | string |
| `technologyTags` | string[] |
| `potentialMarkets` | string[] |
| `commercialOpportunities` | string[] |

### Example response

```json
{
  "title": "Battery Efficiency Optimization",
  "patentOwner": "Example Corp",
  "patentStatus": "Active",
  "technologyTags": ["Energy Storage", "Battery"],
  "potentialMarkets": ["Electric Vehicles", "Grid Storage"],
  "commercialOpportunities": ["Licensing to EV manufacturers"]
}
```

**Note:** List fields may be empty if OpenAI is not configured, the call fails, or the model response is not valid JSON for the expected shape.

**Persistence:** After the report is built (including metadata overlay), it is saved to PostgreSQL (`patent_analyses`). If the database is unavailable, the endpoint still returns **200** with the same JSON body; a warning is logged.

---

## GET /api/patents/history

Returns up to the **20** most recent persisted analyses, newest first.

### Response

Array of `PatentAnalysisHistoryItem`:

| Field | Type |
|--------|------|
| `id` | uuid |
| `patentNumber` | string \| null |
| `title` | string |
| `patentOwner` | string |
| `patentStatus` | string |
| `technologyTags` | string[] |
| `potentialMarkets` | string[] |
| `commercialOpportunities` | string[] |
| `createdAt` | datetime (UTC) |

If the database cannot be read, the API returns **200** with an empty array and logs a warning.
