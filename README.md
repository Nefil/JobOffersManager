# JobOffersManager

## Overview
JobOffersManager is a full-stack job offers management system consisting of a REST API backend and a WPF desktop client.
The project focuses on clean architecture, business validation, data querying, and separation of concerns between backend and frontend layers.

## Tech Stack
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- WPF (MVVM pattern)
- xUnit
- Swagger / OpenAPI
- Asynchronous EF Core operations


## Architecture
The application follows a layered architecture:

Controller → Service → DbContext → Database

- Controllers handle HTTP requests
- Services contain business logic and validation
- DbContext handles data access via EF Core
- Middleware handles global error handling
- DTOs define API contracts

The WPF client follows MVVM:
View → ViewModel → API Service → API

- Views are responsible for UI
- ViewModels handle UI logic and data binding
- API Service communicates with the backend API
- ObservableCollection updates UI dynamicly

## Main Features
Backend:
- CRUD operations for job offers
- DTO-based API contracts
- Business-level validation in service layer
- Global exception handling middleware
- Filtering by location and seniority
- Sorting by title and created date
- Pagination with total count metadata
- EF Core migrations
- Asynchronous database operations
- Unit tests using InMemory database

WPF Client:
- DataGrid with custom columns
- Pagination controls (Next / Previous)
- Dynamic filtering from UI
- Add / Edit / Delete job offers
- Form validation
- Error handling with MessageBox
MVVM-based structure

## API Example
GET /api/jobs?location=gli&seniority=junior&page=1&pageSize=10


Returns a paginated and filtered list of job offers with metadata including total count.

## Project Status

### Completed:
- ✅ Backend API with filtering, sorting and pagination
- ✅ WPF client using MVVM pattern
- ✅ Unit tests for service layer
- ✅ CRUD operations with validation
- ✅ Global exception handling
- ✅ Two-way data binding
- ✅ Command pattern implementation

### Possible Future Improvements:
- Authentication and authorization
- Logging (Serilog)
- Docker support
- Cloud deployment (Azure)
- Advanced sorting options in UI
- Export to CSV/PDF
- Search history
- Caching

## How to Run
1. Clone the repository
2. Open solution in Visual Studio
3. Run the API project
4. Swagger UI will be available at /swagger
5. Use the following operations:
- **Load** - Load job offers from API
- **Add** - Create new job offer
- **Edit Selected** - Modify existing job offer
- **Delete Selected** - Remove job offer
- **Search** - Filter by location and seniority
- **Next/Previous** - Navigate between pages

## License
This project is on MIT License.
