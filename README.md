# Arbox.EmployeeManagement

A .NET 9 ASP-NET Core MVC application for managing employees and departments, backed by Entity Framework Core and SQL Server. It follows Clean Architecture principles, cleanly separating Core (domain), Application (services/interfaces), Infrastructure (EF Core, repositories), and Presentation (MVC, controllers/views). Features include full CRUD for Employees and Departments, pagination, sorting, filtering, and a dashboard with key statistics.

---

## Table of Contents

- [Description](#description)  
- [Folder Structure](#folder-structure)  
- [Prerequisites](#prerequisites)  
- [Installation](#installation)  
- [Running the Application](#running-the-application)  
- [Features & Routes](#features--routes)  
- [Architecture Overview](#architecture-overview)  
- [Technologies Used](#technologies-used)  
- [Docker Setup (optional)](#docker-setup-optional)  

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

1. **Clone the repo**
   ```bash
   git clone https://github.com/MaorTe/Arbox.EmployeeManagement.git
   cd Arbox.EmployeeManagement
   ```

2. **Configure your database**
   Open `.env` and set your connection string under `"ConnectionStrings:Default"`:
   ```json
   "ConnectionStrings": {
     "Default": "Server=localhost;Database=ArboxDb;User Id=sa;Password=YourPassword;TrustServerCertificate=true"
   }
   ```

3. **Restore & build**
   ```bash
   dotnet restore
   dotnet build
   ```

## Apply EF Core Migrations (host-side)

1. **Open PowerShell (or CMD)** in your solution root (where you see `docker-compose.yml` and the `src` folder).

2. **Point EF at your Dockerized database** by exporting the connection string into `ConnectionStrings__Default`:
   ```powershell
   $Env:ConnectionStrings__Default = "Server=localhost,1433;Database=ArboxDb;User Id=sa;Password={YourPassword};TrustServerCertificate=True"
   ```

3. **Change directory** into the project that contains your Migrations (Infrastructure):
   ```powershell
   cd src
   ```

4. **Apply EF Core migrations**
   ```bash
   dotnet ef migrations add <MigrationName> --project Infrastructure --startup-project Web
   ```

5. **Change directory** into the project that contains our Web:
   ```powershell
   cd Web
   ```

6. **Apply all pending migrations**:
   ```powershell
   dotnet ef database update --startup-project ..\Web\Web.csproj
   ```

   You should see:
   ```
   Applying migration '20250805_InitialCreate'.
   Done.
   ```
   **Verify** in SSMS under **ArboxDb → Tables** that your tables now exist.
---

## Restart your Web container

```bash
docker-compose up -d --no-deps web
```

---

## Running the Application

### Via CLI

```bash
cd src/Web
dotnet run
```

By default it listens on the URLs in `launchSettings.json` (e.g. `http://localhost:5246`).

### Via Visual Studio / VS Code

1. Open `Arbox.EmployeeManagement.sln`.
2. Set the **Web** project as startup.
3. Run (F5) — the browser will open to the default route.

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
