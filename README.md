# JobOffersManager

## Overview
JobOffersManager is a backend REST API for managing job offers.
The project focuses on clean architecture, business validation, and efficient data querying.

## Tech Stack
- .NET
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- Swagger / OpenAPI

## Architecture
The application follows a layered architecture:

Controller → Service → DbContext → Database

- Controllers handle HTTP requests
- Services contain business logic and validation
- DbContext handles data access via EF Core
- Middleware handles global error handling

## Main Features
- CRUD operations for job offers
- DTO-based API contracts
- Business-level validation in service layer
- Global exception handling middleware
- Filtering and pagination for job offers
- Partial text search using EF.Functions.Like

## API Example
GET /api/jobs?location=gli&seniority=junior&page=1&pageSize=10


Returns a paginated list of job offers matching the filters.

## Project Status
The project is under active development.
Next planned improvements:
- Sorting
- Unit tests
- API response metadata (total count)

## How to Run
1. Clone the repository
2. Open solution in Visual Studio
3. Run the API project
4. Swagger UI will be available at /swagger
