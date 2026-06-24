[English](README.md) | [Русский](README.ru.md)

# Fitness Tracker Platform

![.NET 8](https://img.shields.io/badge/.NET_8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![React 19](https://img.shields.io/badge/React_19-20232A?style=for-the-badge&logo=react&logoColor=61DAFB)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?style=for-the-badge&logo=typescript&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL_18-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![MinIO](https://img.shields.io/badge/MinIO-C7202C?style=for-the-badge&logo=minio&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

> **Масштабируемая full-stack платформа для отслеживания фитнес-активностей и тренировок.**  
> Проект спроектирован с упором на надежность бизнес-логики, производительность и современную интеграцию пользовательского интерфейса. Кодовая база демонстрирует глубокое понимание принципов Domain-Driven Design (DDD), паттерна CQRS, многослойной архитектуры (Clean Architecture) и разработки Single Page Application (SPA).

---

## Архитектурные паттерны и решения

Репозиторий служит подтверждением владения продвинутыми паттернами проектирования корпоративных систем на всех уровнях:

### 1. Domain-Driven Design (DDD)
Доменный слой (`FitnessTracker.Core`) полностью изолирован от инфраструктуры и внешних зависимостей.
*   **Rich Domain Model**: Отказ от "анемичных" моделей (Anemic Domain Model). Сущности (`Workout`, `Exercise`, `Set`, `User`) полностью инкапсулируют свое состояние. Мутации происходят только через специализированные методы (`AddExercise`, `UpdateSet`).
*   **Value Objects**: Для базовых типов данных созданы объекты-значения (`Calories`, `Repetitions`, `Weight`, `WorkoutDuration`). Это гарантирует, что сущность всегда находится в валидном состоянии (например, вес или количество повторений не могут быть отрицательными на уровне системы типов).
*   **Aggregate Roots**: Четко определены границы транзакций. Работа с упражнениями (`Exercise`) и подходами (`Set`) осуществляется строго через корень агрегации (`Workout`).

### 2. CQRS (Command Query Responsibility Segregation)
Паттерн реализован с помощью `MediatR` с физическим разделением подходов к работе с БД:
*   **Commands (Запись)**: Выполняются через паттерн Repository и `UnitOfWork` с использованием **Entity Framework Core**. Это обеспечивает надежное отслеживание изменений (Change Tracking) и сохранение целостности доменных агрегатов.
*   **Queries (Чтение)**: Реализованы с использованием микро-ORM **Dapper**. Сырые SQL-запросы (`GetWorkoutListHandler`, `GetExerciseHistoryHandler`) позволяют избежать накладных расходов EF Core, исключить загрузку лишних связей и добиться максимальной производительности при сложных выборках и агрегациях.

### 3. Pipeline Behaviors & Result Pattern
*   **Отказ от Exceptions для бизнес-логики**: Реализован паттерн `Result<T>` и `Error` (Railway-Oriented Programming). Ошибки (NotFound, Validation, Conflict) обрабатываются явно и трансформируются в HTTP-ответы централизованно.
*   **Сквозной функционал (MediatR Behaviors)**:
    *   `TransactionBehavior`: Автоматически открывает и коммитит транзакцию БД для всех реализующих `IBaseCommand` команд.
    *   `ValidationBehavior`: Выполняет пайплайн проверок через `FluentValidation`.
    *   `SecureBehavior`: Ограничивает доступ, гарантируя, что пользователь может взаимодействовать только со своими тренировками (Tenant Isolation).

### 4. Frontend Архитектура (React & TypeScript)
*   **Строго типизированное SPA**: Разработано на базе React 19 с использованием TypeScript, что обеспечивает безопасность на этапе компиляции и строгое соответствие моделей данных серверным DTO.
*   **Интеграция с API**: Используется централизованный клиент Axios с настроенными перехватчиками (interceptors) для автоматической инъекции JWT Bearer токенов в заголовки защищенных маршрутов.
*   **Современная сборка и стилизация**: Применяется Vite для быстрой сборки модулей и Tailwind CSS для создания адаптивного компонентного интерфейса (utility-first подход).

---

## Стек технологий

| Слой / Назначение | Технологии |
| --- | --- |
| **Backend** | C#, ASP.NET Core Web API 8.0, MediatR, FluentValidation |
| **Frontend** | React 19, TypeScript, Vite, Tailwind CSS, Axios |
| **Запись (ORM)** | Entity Framework Core 8 |
| **Чтение (Micro-ORM)**| Dapper |
| **База данных** | PostgreSQL 18 |
| **Файловое хранилище**| MinIO (S3 API) для хранения фотографий прогресса |
| **Безопасность** | JWT Bearer Authentication, BCrypt.Net (хэширование) |
| **Инфраструктура** | Docker, Docker Compose, Nginx |

---

## Структура API

API предоставляет безопасный RESTful интерфейс. Все маршруты (кроме аутентификации) защищены JWT.

| Ресурс | Эндпоинты | Функционал |
| --- | --- | --- |
| **Auth** | `POST /api/auth/register`, `POST /api/auth/login` | Регистрация и аутентификация |
| **Workouts** | `GET /api/workouts`, `POST /api/workouts` | Получение списка тренировок и создание новых |
| **Exercises**| `POST /api/workouts/{id}/exercises` | Добавление упражнений в тренировку |
| **Sets** | `POST /api/workouts/{id}/exercises/{eid}/sets` | Трекинг подходов (вес, повторения) |
| **History** | `GET /api/workouts/history` | Агрегация истории по конкретному упражнению (через Dapper) |
| **Media** | `POST /api/workouts/{id}/photos` | Загрузка фотографий в S3-хранилище (MinIO) |

<details>
<summary>Пример ответа: Детали тренировки (Dapper Query)</summary>

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
Локальное развертывание

Проект полностью контейнеризирован, включая многоэтапную сборку (multi-stage build) фронтенда для раздачи через Nginx. Для запуска требуется только Docker Desktop.
Клонируйте репозиторий:  

    git clone https://github.com/ВашПользователь/romansevryuk375-fitness-tracker-api.git
    cd romansevryuk375-fitness-tracker-api
Запустите инфраструктуру (PostgreSQL, MinIO) и само приложение:  

    docker compose up -d --build
Доступ к сервисам:  
        Web UI (Frontend): http://localhost:3000  
        Swagger UI (API Docs): http://localhost:8090/swagger  
        MinIO Console (S3 Admin): http://localhost:9003  
            Login: minio_admin  
            Password: minio_password  
    Примечание: Миграции базы данных применяются автоматически при старте приложения.
