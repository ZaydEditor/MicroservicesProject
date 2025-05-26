# MicroservicesProject
# 🧱 Microservices Project – Product & Order Service (ASP.NET Core/.NET 8)

This repository contains a **microservices-based application** built using **ASP.NET Core (.NET 8)**. The solution consists of two independent services:

- 📦 **Product Service** – Manages products and stock
- 🛒 **Order Service** – Handles customer orders and communicates with the Product Service to check stock availability

---

## 🧰 Tech Stack

- **.NET 8**
- **HTTP Rest** (Communication format)
- **PostgreSQL** (Database)
- **Serilog** (Structured logging)
- **xUnit + Moq + AutoMocker + FluentAssertions** (Unit testing)
- **Swagger** (API documentation for REST endpoints)

---

## 📦 Product Service

### 📍 Endpoints

| Method | Route                         | Description                    |
|--------|-------------------------------|--------------------------------|
| GET    | `/products/{id}`              | Get product by ID              |
| POST   | `/products`                   | Add a new product              |
| PUT    | `/products/{id}/stock`        | Update product stock quantity  |

### 🗃️ Database Fields

- `Id` (int)
- `Name` (string)
- `Description` (string)
- `Price` (decimal)
- `Stock` (int)

---

## 🛒 Order Service

### 📍 Endpoints

| Method | Route         | Description            |
|--------|---------------|------------------------|
| POST   | `/orders`     | Create new order       |
| GET    | `/orders/{id}`| Get order by ID        |

### 🔁 Order Workflow

1. Receives an order request
2. Calls Product Service via HTTP
3. Checks stock availability
4. Reserves stock
5. Saves the order in the database

### 🗃️ Database Fields

- `Id` (int)
- `ProductId` (GUID)
- `Quantity` (int)
- `TotalPrice` (decimal)
- `CreatedAt` (DateTime)

---


