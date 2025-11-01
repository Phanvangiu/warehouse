# Warehouse Management System

A warehouse management web application built with **.NET 9** and **Entity Framework Core**, providing full CRUD functionality for managing warehouse positions, stores, and users.
The project applies the **Repository Pattern + Unit of Work** architecture, ensuring clean code, easy scalability, and maintainability.

## 🚀 Features

- Manage warehouse positions and roles
- CRUD operations for users, stores, and categories
- Repository and Unit of Work patterns implemented
- RESTful API with error handling and validation
- SQL Server database integration

## 🛠️ Tech Stack

- **Backend:** .NET 9, C#, Entity Framework Core
- **Database:** SQL Server
- **Architecture:** Repository Pattern, Unit of Work
- **API Testing:** Postman
- **Version Control:** Git & GitHub

## 🧩 Technology used

- **.NET 9 (ASP.NET Core Web API)**
- **Entity Framework Core 9**
- **MySQL**
- **JWT Authentication**
- **AutoMapper**
- **BCrypt.Net-Next**
- **MailKit**
- **System.IdentityModel.Tokens.Jwt**
- **Repository Pattern + Unit of Work**

---

## 📁 Project Structure

warehouse/
│
├── Controllers/ # API controllers

├── Data/ # DataContext và config database

├── Interfaces/ # Repository and Unit of Work interfaces

├── Models/ # Database mapping entities

├── Repositories/ # Logic processing and data manipulation

├── RequestModels/ # Model received from client

├── ReturnModels/ # CustomResult, DTO,...

├── Program.cs # Entry point of project

├── appsettings.json # Configure DB connection, JWT,...

## ⚙️ Getting Started

### 1️⃣ Prerequisites

Need to install first:

- [.NET SDK 9.0+](https://dotnet.microsoft.com/en-us/download)
- [MySQL](https://dev.mysql.com/downloads/)
- Visual Studio 2022 or VS Code

---

### 2️⃣ Clone project from GitHub

```bash
git clone https://github.com/Phanvangiu/warehouse.git
cd warehouse
```

---

### 3️⃣ Install dependent libraries

```bash
dotnet restore
```

---

### 4️⃣ Configure database connection

Edit the `appsettings.json` file to configure your MySQL connection details:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=warehouse_db;user=root;password=yourpassword;"
}
```

### 5️⃣ Create database & migration

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

### 6️⃣ Run project

```bash
dotnet run
```

By default the application will run at:

```
https://localhost:5001
http://localhost:5000
```

---

### 7️⃣ Access Swagger UI

Open browser:

```
https://localhost:5001/swagger
```

Here you can test all APIs like `Get`, `Post`, `Put`, `Delete`.

---

## 🧠 Overall architecture

This project follows the Repository Pattern and Unit of Work architecture, ensuring a clear separation between layers:

---

Controller → UnitOfWork → Repository → DataContext → Database

---

Controller – Receives requests, validates input data, and calls the UnitOfWork.
Repository – Handles business logic and performs database operations.
UnitOfWork – Groups multiple repositories and manages transactions.
DataContext – Connects to and maps the database using EF Core.

---

## 🧱 CustomResult Format

All APIs return a standardized response format for easier handling on the frontend:

✅ **When successful:**

```json
{
  "status": 200,
  "message": "Position retrieved successfully",
  "data": {
    "id": 1,
    "title": "Manager"
  }
}
```

❌ **When an error occurs:**

```json
{
  "status": 400,
  "message": "Invalid ID. ID must be greater than 0.",
  "data": null
}
```

> ⚙️ The API still returns HTTP 200 (OK), since the actual `status` is managed inside the`CustomResult` object.

---

## 👨‍💻 Author

**Phan Văn Giu**  
📧 Email: <tvtphanvangiu@gmail.com>
💼 GitHub: [https://github.com/Phanvangiu](https://github.com/Phanvangiu)

---

## 🏁 License

The project is developed for learning and internal research purposes.
It must not be used for commercial purposes without the author’s permission.`
