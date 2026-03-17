# API Documentation

## POST /api/patents/analyze

Description:

Analyzes a patent and generates a commercialization report.

Request Body:

PatentAnalysisRequest

Fields:

PatentNumber (optional)
Abstract (optional)

Example:

{
  "PatentNumber": "US1234567"
}

or

{
  "Abstract": "A new method for improving lithium battery efficiency."
}

Validation:

If both fields are missing → HTTP 400

Response:

CommercialReport

Example response:

{
  "Title": "Battery Efficiency Optimization",
  "PatentOwner": "Example Corp",
  "PatentStatus": "Active",
  "TechnologyTags": ["Energy Storage", "Battery"],
  "PotentialMarkets": ["Electric Vehicles", "Grid Storage"],
  "CommercialOpportunities": ["Licensing to EV manufacturers"]
}
