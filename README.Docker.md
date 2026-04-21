# Docker Setup for ASP.NET Core 9 MongoDB Repository Pattern

This guide explains how to run the application using Docker and Docker Compose.

## Prerequisites

- Docker Desktop installed on your system
- Docker Compose (included with Docker Desktop)

## Quick Start

### 1. Build and Run with Docker Compose

From the project root directory, run:

```bash
docker-compose up --build
```

This command will:
- Build the ASP.NET Core 9 application Docker image
- Pull the MongoDB image
- Start both containers
- Create a network for communication between containers
- Set up a persistent volume for MongoDB data

### 2. Access the Application

- **API**: http://localhost:5000
- **API (HTTPS)**: http://localhost:5001
- **MongoDB**: localhost:27017

### 3. Test the API

You can test the API endpoints using curl, Postman, or the provided `.http` file:

```bash
# Get all products
curl http://localhost:5000/api/products

# Get product by ID
curl http://localhost:5000/api/products/{id}

# Create a new product
curl -X POST http://localhost:5000/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"Sample Product","price":29.99,"category":"Electronics"}'
```

## Docker Configuration Details

### Services

1. **mongodb**
   - Image: mongo:latest
   - Port: 27017
   - Credentials: admin/password123
   - Data persistence: mongodb_data volume

2. **aspnet-api**
   - Built from Dockerfile
   - Ports: 5000 (HTTP), 5001 (HTTPS)
   - Environment: Development
   - Auto-connects to MongoDB container

### Environment Variables

The application uses these environment variables (configured in docker-compose.yml):

- `ASPNETCORE_ENVIRONMENT`: Set to Development
- `ASPNETCORE_URLS`: HTTP endpoint configuration
- `MongoDbSettings__ConnectionString`: MongoDB connection string
- `MongoDbSettings__DatabaseName`: Database name (MongoDbDemo)

## Useful Commands

### Start containers in detached mode
```bash
docker-compose up -d
```

### Stop containers
```bash
docker-compose down
```

### Stop containers and remove volumes (deletes data)
```bash
docker-compose down -v
```

### View logs
```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f aspnet-api
docker-compose logs -f mongodb
```

### Rebuild containers
```bash
docker-compose up --build
```

### Access MongoDB shell
```bash
docker exec -it mongodb mongosh -u admin -p password123
```

## Troubleshooting

### Port Already in Use
If ports 5000, 5001, or 27017 are already in use, modify the port mappings in `docker-compose.yml`:

```yaml
ports:
  - "5002:8080"  # Change 5000 to 5002
  - "5003:8081"  # Change 5001 to 5003
```

### Container Won't Start
Check logs for errors:
```bash
docker-compose logs aspnet-api
```

### MongoDB Connection Issues
Ensure the MongoDB container is running:
```bash
docker ps
```

### Reset Everything
To start fresh:
```bash
docker-compose down -v
docker-compose up --build
```

## Production Considerations

For production deployment, consider:

1. **Security**: Change default MongoDB credentials
2. **HTTPS**: Configure proper SSL certificates
3. **Environment**: Set `ASPNETCORE_ENVIRONMENT=Production`
4. **Secrets**: Use Docker secrets or environment variable files
5. **Monitoring**: Add health checks and logging
6. **Backup**: Implement MongoDB backup strategy

## Network Architecture

```
┌─────────────────────────────────────┐
│   Docker Network (bridge)           │
│                                     │
│  ┌──────────────┐  ┌─────────────┐ │
│  │  ASP.NET API │  │   MongoDB   │ │
│  │  Port: 8080  │──│  Port: 27017│ │
│  └──────────────┘  └─────────────┘ │
│         │                           │
└─────────┼───────────────────────────┘
          │
    Host Ports
    5000, 5001
```
