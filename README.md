# Arbox.EmployeeManagement

A .NET 9 ASP-NET Core MVC application for managing employees and departments, backed by Entity Framework Core and SQL Server. It follows Clean Architecture principles, cleanly separating Core (domain), Application (services/interfaces), Infrastructure (EF Core, repositories), and Presentation (MVC, controllers/views). Features include full CRUD for Employees and Departments, pagination, sorting, filtering, and a dashboard with key statistics.

---

## Table of Contents

- [Description](#description)  
- [Folder Structure](#folder-structure)  
- [Prerequisites](#prerequisites)  
- [Installation](#installation)  
- [Features & Routes](#features--routes)  
- [Architecture Overview](#architecture-overview)  
- [Technologies Used](#technologies-used)  

---

## Description

This web application provides:

- *Employee CRUD*: List, create, edit, delete employees, with pagination, sorting, and filtering by name/department.  
- *Department CRUD*: List, create, edit, delete departments (with delete‐protection if employees exist).  
- *Dashboard*:  
  - Total employee count.  
  - Employees grouped by department (table or chart-ready data).  
  - Recent hires (last 30 days).  
  - Filter box for quick name/department search.  

It leverages EF Core for data access, SQL Server for storage, and Clean Architecture to keep concerns separated and the codebase maintainable.

---

## Folder Structure
Arbox.EmployeeManagement/<br>
├── Core/&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; # Domain entities and interfaces<br>
│   └── Entities/&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;# Employee.cs, Department.cs<br>
├── Application/&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;# Service interfaces<br>
│   └── Interfaces/&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;# IEmployeeService.cs, IDepartmentService.cs, IDashboardService.cs<br>
├── Infrastructure/&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;# EF Core DbContext, repositories, services<br>
│   ├── Data/&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;# AppDbContext.cs, EF configurations<br>
│   ├── Migrations/&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;# Migration related files<br>
│   ├── Repositories/&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;# EmployeeRepository.cs, DepartmentRepository.cs<br>
│   └── Services/&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;# EmployeeService.cs, DepartmentService.cs, DashboardService.cs<br>
├── Web/&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;# Presentation layer (ASP-NET Core MVC)<br>
│   ├── Controllers/&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;# EmployeeController.cs, DepartmentController.cs, DashboardController.cs<br>
│   ├── ViewModels/&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;# EmployeeIndexViewModel.cs, EmployeeCreateViewModel.cs, DashboardViewModel.cs<br>
│   ├── Views/&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;# Razor views (Employee/, Department/, Dashboard/, Shared/_Layout.cshtml, Error/)<br>
│   ├── Logs/&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;# Error logs written to files<br>
│   ├── Middleware/&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;# ExceptionLoggingMiddleware.cs<br>
│   └── Program.cs&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;# Host setup, DI, middleware pipeline<br>
│   └── Dockerfile&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;# Assemble Docker container’s filesystem and runtime<br>
├── docker-compose.yml&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;# Bring-up with SQL Server & Web<br>
└── Arbox.EmployeeManagement.sln&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;# Solution file

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (local or Docker)
- (Optional) [Entity Framework Core CLI tools](https://learn.microsoft.com/ef/core/cli/dotnet)

---

## Installation

**Clone the repo**
   ```bash
   git clone https://github.com/MaorTe/Arbox.EmployeeManagement.git
   cd Arbox.EmployeeManagement
   ```

Docker-only (preferred)
-----------------------
Prereqs: Docker
```
Run:
  docker compose up --build

Open:
  http://localhost:5246
```

What happens:
- SQL Server + API run in containers.
- Connection string is injected by Compose (Server=db,1433;...).
- The app auto-applies EF Core migrations on startup—no EF CLI needed.

Only if needed - Reset DB (clean slate):
  docker compose down -v
  docker compose up --build

Troubleshooting:
- API logs: docker compose logs -f web
- DB logs:  docker compose logs -f db
- If web starts before DB is fully ready, wait a few seconds—transient retries + auto-migrate will complete.

Local API + Docker DB
---------------------
Use this only if you want to run dotnet run on your machine.

Prereqs: Docker Desktop, .NET SDK

Validate appsettings.Development.json contains:
  Server=localhost,1433;Database=ArboxDb;User Id=sa;Password=MySecureP@ss!;Encrypt=True;TrustServerCertificate=True

Start DB in Docker
```
Run:
  docker compose up -d db
  dotnet restore
  dotnet run --project src/Web
Open:
  http://localhost:5246
```

Only if needed - Reset DB (clean slate):
  docker compose down -v
  docker compose up -d db

Notes:
- Local run connects to the Docker DB via localhost,1433.
- The app auto-migrates on startup; EF CLI is not required.
- If you previously started the web container, stop it to avoid port conflicts:
    docker compose stop web


Useful Commands (both modes)
```
  docker compose ps                  # show container status
  docker compose logs -f web         # tail API logs
  docker compose logs -f db          # tail DB logs
  docker compose down -v             # stop & delete containers + DB volume
```

What’s Included
- Auto-migrations on startup (Database.Migrate() in Program.cs)
- Docker-only path (DB + API in containers)
- Local + Docker DB path (API on host, DB in container)


---

## Features & Routes

| Feature                         | Method | URL                                     | Description                                                 |
|---------------------------------|--------|-----------------------------------------|-------------------------------------------------------------|
| **Dashboard**                   | GET    | `/Dashboard`                            | Shows total count, by-department grouping, recent hires.    |
| **List Employees**              | GET    | `/Employee`                             | Paginated, sortable list with filter form.                  |
| **Create Employee (form)**      | GET    | `/Employee/Create`                      | Renders form.                                               |
| **Create Employee (submit)**    | POST   | `/Employee/Create`                      | Validates & adds new employee.                              |
| **Edit Employee (form)**        | GET    | `/Employee/Edit/{id}`                   | Renders edit form.                                          |
| **Edit Employee (submit)**      | POST   | `/Employee/Edit`                        | Validates & updates.                                        |
| **Delete Employee**             | GET    | `/Employee/Delete/{id}`                 | Deletes after confirmation.                                 |
| **List Departments**            | GET    | `/Department`                           | Shows all departments.                                      |
| **Create Department (form)**    | GET    | `/Department/Create`                    | Renders form.                                               |
| **Create Department (submit)**  | POST   | `/Department/Create`                    | Validates & adds.                                           |
| **Edit Department (form)**      | GET    | `/Department/Edit/{id}`                 | Renders edit form.                                          |
| **Edit Department (submit)**    | POST   | `/Department/Edit`                      | Validates & updates.                                        |
| **Delete Department**           | GET    | `/Department/Delete/{id}`               | Blocks if employees exist, otherwise deletes.               |
| **Error Page**                  | GET    | `/Error`                                | Friendly error page for unhandled exceptions.               |

---

## Architecture Overview

- **Core**: Domain entities (`Employee`, `Department`).
- **Application**: Service interfaces (`IEmployeeService`, etc.).
- **Infrastructure**: EF Core (`AppDbContext`), repository implementations, services.
- **Web**: MVC controllers & Razor views, middleware for logging & error handling.

Dependencies flow inward:

Web → Infrastructure → Application → Core

---

## Technologies Used

- .NET 9 & C# 12
- ASP-NET Core MVC
- Entity Framework Core
- SQL Server
- Clean Architecture principles
- Bootstrap 5
- Custom middleware (exception logging, error pages)
