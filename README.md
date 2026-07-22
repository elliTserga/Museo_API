# MuseoAPI

## Overview

MuseoAPI is an ASP.NET Core Web API developed as part of my internship. The API provides the backend functionality for a museum management system, allowing clients to manage exhibits, categories, announcements, media items and museum information.

The project uses JWT authentication for protected endpoints and stores data in Microsoft SQL Server.

---

## Technologies

- ASP.NET Core
- C#
- Dapper
- Microsoft SQL Server
- JWT Authentication
- BCrypt Password Hashing

---

## Project Structure

The solution is split into multiple projects:

- **MuseoAPI** – Controllers, middleware and API configuration.
- **MuseoData** – Data access layer (repositories).
- **Adapter** – Database connection management.
- **MuseoAuth** – Authentication, JWT generation and password hashing.
- **MuseoShared** – Shared models and DTOs.

---

## Database

The `Database` folder contains everything required to recreate the database:

- `01_CreateTables.sql`
- `02_SeedData.sql`
- `03_CreateStoredProcedures.sql`
- `MuseoDb_Schema.sql`

A full database backup (`MuseoDb.bak`) can also be used to restore the complete database including data.

---

## Configuration

Sensitive information is stored in a `.env` file and is **not** included in the repository.

Create a `.env` file using the provided `.env.example`.

Example:

```env
ConnectionStrings__DefaultConnection=...

Jwt__Key=...
Jwt__Issuer=...
Jwt__Audience=...
JWT_EXPIRATION_HOURS=2
```

---

## Authentication

The API uses JWT authentication.

Login endpoint:

```
POST /api/auth/login
```

Example request:

```json
{
    "username": "admin",
    "password": "admin123"
}
```

The returned JWT token must be included as a Bearer Token when accessing protected endpoints.

---

## Features

- JWT Authentication
- CRUD operations for Exhibits
- CRUD operations for Categories
- CRUD operations for Announcements
- CRUD operations for Media Items
- Museum information management
- Filter exhibits by category
- Exhibits with associated media
- Announcement visibility and scheduling
- SQL Stored Procedures
- Global exception handling

---
