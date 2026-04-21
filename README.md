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
- [Project Structure](#project-structure)
- [Architecture](#architecture)
- [Contributing](#contributing)
- [License](#license)

## ✨ Features

- ✅ RESTful API with CRUD operations
- ✅ Repository Pattern implementation
- ✅ MongoDB integration with async operations
- ✅ Generic repository for code reusability
- ✅ Dependency Injection
- ✅ OpenAPI/Swagger documentation
- ✅ Docker support with Docker Compose
- ✅ Clean Architecture principles
- ✅ ASP.NET Core 9 with minimal APIs

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

### Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get product by ID |
| GET | `/api/products/GetByPriceRange/{minPrice}/{maxPrice}` | Get products by price range |
| POST | `/api/products` | Create a new product |
| PUT | `/api/products/{id}` | Update a product |
| DELETE | `/api/products/{id}` | Delete a product |

### Example Requests

**Create a Product**
```bash
curl -X POST http://localhost:5000/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Laptop",
    "price": 999.99
  }'
```

**Get All Products**
```bash
curl http://localhost:5000/api/products
```

**Get Products by Price Range**
```bash
curl http://localhost:5000/api/products/GetByPriceRange/100/1000
```

For more detailed API documentation, see [API Reference](docs/API.md).

## 📁 Project Structure

```
Market-Api/
├── Market.API/
│   ├── Controllers/          # API Controllers
│   ├── Entities/             # Domain Models
│   ├── Repository/           # Repository Pattern Implementation
│   ├── Settings/             # Configuration Settings
│   ├── Program.cs            # Application Entry Point
│   └── appsettings.json      # Configuration File
├── docs/                     # Documentation
│   ├── API.md               # API Documentation
│   ├── ARCHITECTURE.md      # Architecture Guide
│   ├── DOCKER.md            # Docker Guide
│   └── CONTRIBUTING.md      # Contribution Guidelines
├── .github/                  # GitHub Configuration
│   ├── workflows/           # CI/CD Workflows
│   ├── ISSUE_TEMPLATE/      # Issue Templates
│   └── PULL_REQUEST_TEMPLATE.md
├── docker-compose.yml        # Docker Compose Configuration
├── Dockerfile               # Docker Image Definition
└── README.md                # This File
```

## 🏗 Architecture

This project follows the **Repository Pattern** with **Dependency Injection**:

- **Controllers**: Handle HTTP requests and responses
- **Repository Layer**: Abstracts data access logic
- **Entities**: Define domain models
- **Generic Repository**: Provides common CRUD operations

For detailed architecture documentation, see [Architecture Guide](docs/ARCHITECTURE.md).

## 🤝 Contributing

Contributions are welcome! Please read our [Contributing Guidelines](docs/CONTRIBUTING.md) before submitting a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Contact

Mostafa SAID - [@Mostafa-SAID7](https://github.com/Mostafa-SAID7)

Project Link: [https://github.com/Mostafa-SAID7/Market-Api](https://github.com/Mostafa-SAID7/Market-Api)

## 🙏 Acknowledgments

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [MongoDB .NET Driver](https://mongodb.github.io/mongo-csharp-driver/)
- [Repository Pattern](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)
