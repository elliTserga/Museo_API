# MuseoAPI

## Overview

MuseoAPI is an ASP.NET Core Web API developed as part of my internship. The API provides the backend functionality for a museum management system, allowing clients to manage exhibits, categories, announcements, media items and museum information.

The project uses JWT authentication for protected endpoints, stores data in Microsoft SQL Server and supports media file uploads through MinIO object storage.

---

## Technologies

- ASP.NET Core
- C#
- Dapper
- Microsoft SQL Server
- JWT Authentication
- BCrypt Password Hashing
- MinIO Object Storage
- Docker

---

## Project Structure

The solution is split into multiple projects:

- **MuseoAPI** – Controllers, middleware, request models and API configuration.
- **MuseoData** – Data access layer (repositories).
- **Adapter** – Database connection management and MinIO storage implementation.
- **MuseoAuth** – Authentication, JWT generation and password hashing.
- **MuseoShared** – Shared models, DTOs and interfaces.

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

Minio__Endpoint=localhost:9000
Minio__AccessKey=minioadmin
Minio__SecretKey=minioadmin
Minio__BucketName=media
Minio__UseSSL=false
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

## File Storage

Media files are uploaded using `multipart/form-data`.

The uploaded file is stored in MinIO object storage, while only its metadata (file name, content type, file size and storage path) is stored in the SQL Server database.

Upload request:

| Field | Type |
|------|------|
| ExhibitId | Text |
| file | File |

Before running the application, make sure the MinIO container is running and the configured bucket has been created.

---

## Features

- JWT Authentication
- Password hashing using BCrypt
- CRUD operations for Exhibits
- Categories management
- Museum information
- Announcements
- Media items
- File upload using MinIO
- Media metadata storage in SQL Server
- SQL Stored Procedures
- Global exception handling

---