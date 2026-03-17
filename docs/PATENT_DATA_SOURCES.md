# Patent Data Sources

Project: PatentClarity  
Backend: PCBack  
Date: March 17, 2026

---

# Purpose

PatentClarity needs reliable patent data sources to retrieve patent metadata such as:

• patent title  
• abstract  
• assignee (owner)  
• filing date  
• patent status  

This document lists the main APIs and datasets that can be integrated into the backend.

---

# 1. PatentsView API (Recommended for MVP)

Website:
https://patentsview.org

Description:

PatentsView is an official patent data platform supported by the USPTO.

It provides free access to structured patent metadata.

Advantages:

• Free  
• No API key required  
• JSON API  
• Good documentation  

Example query:

https://api.patentsview.org/patents/query?q={"patent_number":"1234567"}

Example fields:

patent_title  
patent_abstract  
assignee_organization  
patent_date

Use case in PatentClarity:

Used by PatentService to retrieve metadata when a user submits a patent number.

---

# 2. Google Patents

Website:
https://patents.google.com

Description:

Google Patents provides access to worldwide patent data.

Advantages:

• very large dataset
• global coverage

Limitations:

• no official public API
• requires scraping or third-party APIs

Use case:

Possible future data source for global patents.

---

# 3. USPTO Open Data Portal

Website:
https://developer.uspto.gov

Description:

Official patent datasets provided by the United States Patent and Trademark Office.

Advantages:

• official data source
• reliable data

Limitations:

• large datasets
• not optimized for quick API queries

Use case:

Bulk data ingestion for large-scale patent analysis.

---

# 4. The Lens

Website:
https://www.lens.org

Description:

The Lens provides open patent and scholarly data.

Advantages:

• global patent coverage
• good research tools

Limitations:

• API usage limits
• requires registration

Use case:

Future integration for global patent discovery.

---

# Recommended MVP Data Source

For the first version of PatentClarity:

Primary source:

PatentsView API

Reason:

It is free, simple, and reliable for retrieving patent metadata using a patent number.

---

# Future Data Strategy

Future versions of PatentClarity may combine multiple sources:

• PatentsView
• Google Patents
• USPTO datasets
• global patent APIs

This will allow global patent search and deeper analytics.
