[English](README.md) | [Русский](README.ru.md)

# Fitness Tracker Platform

![.NET 8](https://img.shields.io/badge/.NET_8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![React 19](https://img.shields.io/badge/React_19-20232A?style=for-the-badge&logo=react&logoColor=61DAFB)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?style=for-the-badge&logo=typescript&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL_18-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![MinIO](https://img.shields.io/badge/MinIO-C7202C?style=for-the-badge&logo=minio&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

> **A scalable full-stack platform for tracking fitness activities and workouts.**  
> This project is designed with a strict focus on business logic reliability, performance, and modern user interface integration. It demonstrates a deep understanding of Domain-Driven Design (DDD) principles, the CQRS pattern, Clean Architecture, and Single Page Application (SPA) development.

---

## Architectural Patterns and Solutions

This repository serves as a demonstration of proficiency in advanced enterprise design patterns across the entire stack:

### 1. Domain-Driven Design (DDD)
The domain layer (`FitnessTracker.Core`) is completely isolated from the infrastructure and external dependencies.
*   **Rich Domain Model**: Rejection of the Anemic Domain Model anti-pattern. Entities (`Workout`, `Exercise`, `Set`, `User`) fully encapsulate their state. State mutations occur solely through specialized domain methods (`AddExercise`, `UpdateSet`).
*   **Value Objects**: Implemented for base data types (`Calories`, `Repetitions`, `Weight`, `WorkoutDuration`). This guarantees that an entity is always in a valid state (e.g., weight or repetition count cannot be negative at the type system level).
*   **Aggregate Roots**: Transaction boundaries are strictly defined. Interactions with `Exercise` and `Set` entities are handled exclusively through the `Workout` aggregate root.

### 2. CQRS (Command Query Responsibility Segregation)
The pattern is implemented using `MediatR` alongside a physical separation of database access strategies:
*   **Commands (Write)**: Executed via the Repository pattern and `UnitOfWork` using **Entity Framework Core**. This ensures reliable Change Tracking and maintains the integrity of domain aggregates.
*   **Queries (Read)**: Implemented using the micro-ORM **Dapper**. Raw SQL queries (`GetWorkoutListHandler`, `GetExerciseHistoryHandler`) bypass EF Core overhead, eliminate unnecessary relationship loading, and achieve maximum performance for complex data retrievals and aggregations.

### 3. Pipeline Behaviors & Result Pattern
*   **No Exceptions for Business Logic Flow**: The `Result<T>` and `Error` pattern (Railway-Oriented Programming) is implemented. Errors (NotFound, Validation, Conflict) are handled explicitly and transformed into appropriate HTTP responses centrally.
*   **Cross-Cutting Concerns (MediatR Behaviors)**:
    *   `TransactionBehavior`: Automatically opens and commits a database transaction for all commands implementing the `IBaseCommand` interface.
    *   `ValidationBehavior`: Executes the validation pipeline via `FluentValidation`.
    *   `SecureBehavior`: Restricts access, guaranteeing that a user can only interact with their own workouts (Tenant Isolation).

### 4. Frontend Architecture (React & TypeScript)
*   **Strictly Typed SPA**: Developed using React 19 and TypeScript, ensuring compile-time safety and robust data models mapping backend DTOs.
*   **Centralized API Integration**: Uses a configured Axios instance with request interceptors for automatic JWT Bearer token injection across all protected routes.
*   **Modern Build Tooling & Styling**: Utilizes Vite for rapid module bundling and Tailwind CSS for a utility-first, highly responsive component-based UI design.

---

## Technology Stack

| Layer / Purpose | Technologies |
| --- | --- |
| **Backend** | C#, ASP.NET Core Web API 8.0, MediatR, FluentValidation |
| **Frontend** | React 19, TypeScript, Vite, Tailwind CSS, Axios |
| **Write (ORM)** | Entity Framework Core 8 |
| **Read (Micro-ORM)**| Dapper |
| **Database** | PostgreSQL 18 |
| **File Storage**| MinIO (S3 API) for storing progress photos |
| **Security** | JWT Bearer Authentication, BCrypt.Net (hashing) |
| **Infrastructure** | Docker, Docker Compose, Nginx |

---

## API Structure

The API provides a secure RESTful interface. All routes (except authentication) are protected by JWT.

| Resource | Endpoints | Functionality |
| --- | --- | --- |
| **Auth** | `POST /api/auth/register`, `POST /api/auth/login` | Registration and authentication |
| **Workouts** | `GET /api/workouts`, `POST /api/workouts` | Retrieve workout list and create new ones |
| **Exercises**| `POST /api/workouts/{id}/exercises` | Add an exercise to a workout |
| **Sets** | `POST /api/workouts/{id}/exercises/{eid}/sets` | Track sets (weight, repetitions) |
| **History** | `GET /api/workouts/history` | Aggregate history for a specific exercise (via Dapper) |
| **Media** | `POST /api/workouts/{id}/photos` | Upload photos to S3-compatible storage (MinIO) |

<details>
<summary>Example Response: Workout Details (Dapper Query)</summary>

```json
{
  "id": "e3b0c442-989b-464b-8b4e-1234567890ab",
  "title": "Chest & Triceps",
  "type": "Strength",
  "duration": "01:15:00",
  "caloriesBurned": 450,
  "workoutDate": "2026-06-13T10:00:00Z",
  "exercises": [
    {
      "id": "f5d0c442-989b-464b-8b4e-1234567890ab",
      "exerciseName": "Bench Press",
      "sets": [
        { "reps": 10, "weight": "80 kg" },
        { "reps": 8, "weight": "85 kg" }
      ]
    }
  ],
  "photos": []
}

```

</details>
Local Deployment

The project is fully containerized, featuring a multi-stage Docker build for the frontend via Nginx. Running it requires only Docker Desktop.
    Clone the repository:

    git clone https://github.com/YourUsername/romansevryuk375-fitness-tracker-api.git
    cd romansevryuk375-fitness-tracker-api

Spin up the infrastructure (PostgreSQL, MinIO) and the application services:

    docker compose up -d --build

 Access the services:  
        Web UI (Frontend): http://localhost:3000  
        Swagger UI (API Docs): http://localhost:8090/swagger  
        MinIO Console (S3 Admin): http://localhost:9003  
            Login: minio_admin  
            Password: minio_password  
    Note: Database migrations are applied automatically upon application startup.
