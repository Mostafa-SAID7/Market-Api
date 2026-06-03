# Market API

A modern RESTful API built with ASP.NET Core 9 and MongoDB, implementing the Repository Pattern for clean architecture and maintainable code.

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![MongoDB](https://img.shields.io/badge/MongoDB-3.0-47A248?logo=mongodb)](https://www.mongodb.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## 📋 Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Configuration](#configuration)
- [Running the Application](#running-the-application)
- [API Documentation](#api-documentation)
- [Documentation Guide](#documentation-guide)
- [Architecture](#architecture)
- [Contributing](#contributing)
- [License](#license)

## ✨ Features

- ✅ **MediatR Pattern** - CQRS architecture with commands and queries
- ✅ **Complete REST API** - CRUD operations for 7 entities
- ✅ **Validation Framework** - Strong entity validation
- ✅ **Service Layer** - Business logic with logging
- ✅ **Repository Pattern** - Clean data access abstraction
- ✅ **Unit of Work** - Coordinated data operations
- ✅ **MongoDB Integration** - Async NoSQL operations
- ✅ **Dependency Injection** - Full DI support
- ✅ **Swagger/OpenAPI** - Interactive API documentation
- ✅ **Docker Support** - Docker & Docker Compose ready
- ✅ **Clean Architecture** - SOLID principles throughout
- ✅ **Comprehensive Logging** - Serilog integration ready

## 🛠 Tech Stack

- **Framework**: ASP.NET Core 9.0
- **Database**: MongoDB 3.0
- **Language**: C# with .NET 9
- **Architecture**: Repository Pattern
- **API Documentation**: OpenAPI/Swagger
- **Containerization**: Docker & Docker Compose

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [MongoDB](https://www.mongodb.com/try/download/community) (or use Docker)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional, for containerized deployment)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Mostafa-SAID7/Market-Api.git
   cd Market-Api
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

### Configuration

Update the MongoDB connection string in `appsettings.json`:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "MongoDbDemo"
  }
}
```

For Docker deployment, the connection string is automatically configured via environment variables.

## 🏃 Running the Application

### Option 1: Run Locally

1. **Start MongoDB** (if not using Docker)
   ```bash
   mongod
   ```

2. **Run the application**
   ```bash
   cd Market.API
   dotnet run
   ```

3. **Access the API**
   - API: `https://localhost:7294` or `http://localhost:5000`
   - Swagger UI: `https://localhost:7294/swagger` (in Development mode)

### Option 2: Run with Docker

See [Docker Documentation](docs/DOCKER.md) for detailed instructions.

```bash
docker-compose up --build
```

Access the API at `http://localhost:5000`

## 📚 API Documentation

### Quick API Reference

| Entity | Create | Read | Update | Delete | Special |
|--------|--------|------|--------|--------|---------|
| Products | POST | GET | PUT | DELETE | GetByCategory |
| Categories | POST | GET | PUT | DELETE | - |
| Users | POST | GET | PUT | DELETE | - |
| Vendors | POST | GET | PUT | DELETE | Approve |
| Orders | POST | GET | PUT | DELETE | - |
| Carts | - | GET | - | - | Add, Remove, Clear |
| Reviews | POST | GET | PUT | DELETE | GetByProduct |

### Complete Documentation

For detailed documentation and guides, see **[docs/DOCUMENTATION.md](docs/DOCUMENTATION.md)** which provides:

- **[API Reference](docs/API.md)** - Complete endpoint documentation with all 7 entities
- **[Architecture Guide](docs/ARCHITECTURE.md)** - System design, patterns, and structure
- **[Implementation Guide](docs/IMPLEMENTATION.md)** - Detailed code patterns and examples
- **[Docker Setup](docs/DOCKER.md)** - Containerization and deployment instructions
- **[Contributing Guidelines](docs/CONTRIBUTING.md)** - Development workflow and standards
- **[Changelog](docs/CHANGELOG.md)** - Version history and updates

### Example Requests

**Get All Products**
```bash
curl http://localhost:5000/api/products
```

**Create Product**
```bash
curl -X POST http://localhost:5000/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Gaming Laptop",
    "description": "High performance laptop",
    "category": "Electronics",
    "vendorId": "507f1f77bcf86cd799439000",
    "price": 1299.99,
    "quantity": 10
  }'
```

**Get User by ID**
```bash
curl http://localhost:5000/api/users/507f1f77bcf86cd799439001
```

**Create Order**
```bash
curl -X POST http://localhost:5000/api/orders \
  -H "Content-Type: application/json" \
  -d '{
    "customerId": "507f1f77bcf86cd799439001",
    "items": [...],
    "shippingAddress": "123 Main St",
    "shippingCost": 50.00,
    "tax": 100.00
  }'
```

### Swagger UI

Access interactive API documentation in development:

```
Development: https://localhost:7294/swagger
Docker: http://localhost:5000/swagger
```

---

## 📖 Documentation Guide

**New to the project?** Start with **[docs/DOCUMENTATION.md](docs/DOCUMENTATION.md)** for a complete navigation guide organized by use case.

### Quick Navigation

| I want to... | Read this |
|-------------|-----------|
| Get started quickly | Start here (README.md) |
| Understand how docs are organized | [docs/DOCUMENTATION.md](docs/DOCUMENTATION.md) |
| Use the API endpoints | [docs/API.md](docs/API.md) |
| Understand the architecture | [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) |
| Implement new features | [docs/IMPLEMENTATION.md](docs/IMPLEMENTATION.md) |
| Deploy with Docker | [docs/DOCKER.md](docs/DOCKER.md) |
| Contribute to the project | [docs/CONTRIBUTING.md](docs/CONTRIBUTING.md) |
| See what's new | [docs/CHANGELOG.md](docs/CHANGELOG.md) |

## 🏗 Architecture

This project follows the **Repository Pattern** with **CQRS using MediatR**:

- **MediatR Layer**: Commands and queries with handlers
- **Service Layer**: Business logic and validation
- **Repository Layer**: Data access abstraction
- **MongoDB**: NoSQL database with async operations

For detailed architecture documentation, see **[docs/ARCHITECTURE.md](docs/ARCHITECTURE.md)**.

## 🤝 Contributing

Contributions are welcome! Please read **[docs/CONTRIBUTING.md](docs/CONTRIBUTING.md)** before submitting a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'feat: add amazing feature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

For detailed contribution guidelines, coding standards, and best practices, see **[docs/CONTRIBUTING.md](docs/CONTRIBUTING.md)**.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Contact

Mostafa SAID - [@Mostafa-SAID7](https://github.com/Mostafa-SAID7)

Project Link: [https://github.com/Mostafa-SAID7/Market-Api](https://github.com/Mostafa-SAID7/Market-Api)

## 🙏 Acknowledgments

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [MongoDB .NET Driver](https://mongodb.github.io/mongo-csharp-driver/)
- [Repository Pattern](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)
