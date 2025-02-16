# UltimateTodoApp - API

## Overview

`excalido` is a **task management API** built with **ASP.NET Core** and **Entity Framework Core**. It provides an organized way to manage **boards, todo lists, and todos** while supporting **user authentication and role-based access control (RBAC)**.

## Features

- **JWT-based Authentication** (Login, Signup, Logout)
- **Role-based Authorization** (Admin, Editor, Viewer)
- **CRUD Operations** for Boards, Lists, and Todos
- **Response Wrapping Middleware** (Standardized API responses)
- **Soft Deletion Support** for Todos & Lists
- **Audit Logging** for Board modifications
- **Scalable Architecture** using Clean Architecture principles

---

## Project Structure

The project follows a **Clean Architecture** approach:

```
📂 UltimateTodoServer
 ┣ 📂 API              # Entry point (Minimal API)
 ┃ ┣ 📂 Endpoints      # API Routes for each entity
 ┃ ┣ 📂 Middleware     # Custom Middlewares (e.g., Response Wrapper)
 ┃ ┣ 📂 Extensions     # RouteGroup extensions (e.g., WithResponseWrapper)
 ┃ ┣ 📜 Program.cs     # API configuration & service registration
 ┃ ┣ 📜 appsettings.json # Configuration (DB, JWT, etc.)
 ┃
 ┣ 📂 Application      # Business logic layer
 ┃ ┣ 📂 Services       # Core business logic (e.g., BoardService)
 ┃ ┣ 📂 DTOs           # Data Transfer Objects (API Models)
 ┃ ┣ 📂 Interfaces     # Interfaces for Repositories & Services
 ┃
 ┣ 📂 Infrastructure   # Data persistence & repositories
 ┃ ┣ 📂 Persistence    # EF Core Database Context
 ┃ ┣ 📂 Repositories   # Data access layer (e.g., BoardRepository)
 ┃
 ┣ 📂 Domain          # Core domain entities
 ┃ ┣ 📂 Entities      # Database models (e.g., Board, Todo, User)
 ┃ ┣ 📂 Enums         # Enum definitions (e.g., RoleEnum)
 ┃
 ┗ 📂 Tests           # Unit & Integration tests
```

---

## Technologies Used

- **.NET 9** (ASP.NET Core Minimal APIs)
- **Entity Framework Core (EF Core)** (PostgreSQL)
- **JWT Authentication** (Microsoft.IdentityModel.Tokens)
- **Docker** (Database containerization)
- **C#** (Primary language)
- **Rider / Visual Studio Code** (Recommended IDEs)

---

## Installation & Setup

### **1️⃣ Clone the repository**

```sh
git clone https://github.com/your-repo/ultimate-todo-app.git
cd ultimate-todo-app/server/UltimateTodoServer
```

### **2️⃣ Set up the Database (Docker)**

Ensure you have Docker installed, then run:

```sh
docker-compose up -d
```

This will start a **PostgreSQL database** container with a persistent volume.

### **3️⃣ Configure Environment Variables**

Edit `appsettings.json` (or use environment variables):

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=taskflow;Username=secureuser;Password=securepassword"
},
"JwtSettings": {
  "Secret": "YourSuperSecretKeyAtLeast32CharactersLong",
  "Issuer": "TaskFlowApi",
  "Audience": "TaskFlowUsers",
  "ExpirationMinutes": 60
}
```

### **4️⃣ Apply Migrations & Seed Database**

Run EF Core migrations:

```sh
dotnet ef database update --project Infrastructure --startup-project API
```

### **5️⃣ Start the API**

```sh
dotnet run --project API
```

---

## API Endpoints

### **🔹 Authentication**

| Method | Endpoint  | Description             |
| ------ | --------- | ----------------------- |
| `POST` | `/login`  | Login and get JWT token |
| `POST` | `/signup` | Register a new user     |
| `POST` | `/logout` | Invalidate JWT token    |

### **🔹 Boards**

| Method   | Endpoint           | Description                    |
| -------- | ------------------ | ------------------------------ |
| `GET`    | `/api/boards`      | Get all boards (user-specific) |
| `POST`   | `/api/boards`      | Create a new board             |
| `GET`    | `/api/boards/{id}` | Get a board by ID              |
| `PUT`    | `/api/boards/{id}` | Update board details           |
| `DELETE` | `/api/boards/{id}` | Soft-delete a board            |

### **🔹 Todos & Lists**

| Method   | Endpoint                                     | Description          |
| -------- | -------------------------------------------- | -------------------- |
| `GET`    | `/api/boards/{boardId}/lists`                | Get lists in a board |
| `POST`   | `/api/boards/{boardId}/lists`                | Create a new list    |
| `GET`    | `/api/boards/{boardId}/lists/{listId}/todos` | Get todos in a list  |
| `POST`   | `/api/boards/{boardId}/lists/{listId}/todos` | Create a todo        |
| `PATCH`  | `/api/todos/{todoId}`                        | Update a todo        |
| `DELETE` | `/api/todos/{todoId}`                        | Soft-delete a todo   |

---

## Security & Authentication

- Uses **JWT Authentication** (Bearer tokens)
- Protects routes via `.RequireAuthorization()`
- Secure password hashing using **bcrypt**
- **Role-based access control (RBAC)** for board sharing

---

## 🏗️ Development Workflow

### **Run Tests**

```sh
dotnet test
```

### **Generate New EF Core Migrations**

```sh
dotnet ef migrations add MigrationName --project Infrastructure --startup-project API
```

### **Apply Migrations**

```sh
dotnet ef database update --project Infrastructure --startup-project API
```

---

## Next Steps

- ✅ Implement **User Profile Management**
- ✅ Add **Email Notifications**
- ✅ Improve **Frontend UI** (React/Next.js Integration)

---

## 📜 License

This project is licensed under the **MIT License**.

---

## 👥 Contributors

- @hexadeciman
- **Contributors welcome!** 🎉 PRs are encouraged!
