# Contributing to Market API

Thank you for your interest in contributing to **Market API**! This document provides guidelines and conventions for submitting code, reporting bugs, and introducing new features.

---

## Code of Conduct

All contributors are expected to adhere to our [Code of Conduct](CODE_OF_CONDUCT.md). Please read it before participating.

---

## Development Workflow

### 1. Prerequisites
- **.NET 9 SDK**: Make sure you have installed [.NET 9](https://dotnet.microsoft.com/download/dotnet/9.0).
- **Docker Desktop**: Required if testing container builds locally (`docker-compose up`).

### 2. Fork and Clone
```bash
git clone https://github.com/<your-username>/Market-Api.git
cd Market-Api
```

### 3. Branching Naming Conventions
Create a feature or bugfix branch using the following naming structure:
- `feature/<short-description>` (e.g. `feature/jwt-authentication`)
- `fix/<short-description>` (e.g. `fix/swagger-nu1603-warning`)
- `docs/<short-description>` (e.g. `docs/update-readme`)

### 4. Code Standards & Testing
- Ensure your code builds without warnings or errors:
  ```bash
  dotnet build Market.sln
  ```
- Run unit and integration tests locally before opening a PR:
  ```bash
  dotnet test Market.sln
  ```

---

## Submitting Pull Requests (PRs)

1. Ensure your PR targets the `main` or `develop` branch.
2. Fill out the [Pull Request Template](PULL_REQUEST_TEMPLATE.md) in full.
3. Link relevant issues using standard closing keywords (e.g. `Closes #12`).
4. Ensure all GitHub Actions status checks pass (Build, Test, CodeQL, Format).
