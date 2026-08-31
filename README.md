<div align="center">

# 🛒 Market API

**A High-Performance, Enterprise-Grade E-Commerce REST API**

[![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![License](https://img.shields.io/badge/license-MIT-blue.svg?style=for-the-badge)](LICENSE)

*Built with clean architecture, CQRS, and SQL Server for maximum scalability.*

</div>

---

## 🎯 Key Highlights

*   **⚡ High Performance**: Powered by **.NET 9** and **SQL Server**.
*   **🏗️ Clean Architecture**: Strictly adheres to the **Repository Pattern** and **CQRS (MediatR)**.
*   **🛡️ Robust Design**: Features a strong Validation Framework, comprehensive logging, and Unit of Work coordination.
*   **🐳 Container Ready**: Fully configured for **Docker** and **Docker Compose**.
*   **🤖 CI/CD Automated**: Protected by GitHub Actions (CodeQL, Dependabot, Semantic Releases, and PR Linters).

---

## 🚀 Getting Started

### 1. Prerequisites
*   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
*   **SQL Server** (LocalDB, Express, or Docker)

### 2. Quick Setup

```bash
# Clone the repository
git clone https://github.com/Mostafa-SAID7/Market-Api.git
cd Market-Api

# Restore packages
dotnet restore
```

### 3. Database Configuration

Update your SQL Server connection string in `Market.API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=MarketApiDb;Trusted_Connection=true;TrustServerCertificate=true;"
}
```

### 4. Run the API

```bash
cd Market.API
dotnet run
```
*   **Swagger UI**: `https://localhost:7294/swagger`

---

## 🧩 Core Architecture

The system is separated into distinct, maintainable layers:

*   **API Layer**: RESTful endpoints and Swagger configuration.
*   **MediatR Layer**: CQRS Commands and Queries.
*   **Service Layer**: Business rules and request validation.
*   **Repository Layer**: Entity Framework Core data access abstractions for SQL Server.

---

## 📚 Documentation & Reference

Looking to dive deeper? Check out our dedicated documentation:

*   [**API Reference**](docs/API.md) - Endpoints for Products, Orders, Users, etc.
*   [**Architecture Guide**](docs/ARCHITECTURE.md) - System design and patterns.
*   [**Docker Setup**](docs/DOCKER.md) - Container deployment guide.
*   [**Contribution Guidelines**](docs/CONTRIBUTING.md) - How to get involved.

---

<div align="center">
  <p>Built with ❤️ by <a href="https://github.com/Mostafa-SAID7">Mostafa SAID</a></p>
</div>
