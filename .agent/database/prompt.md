# MARKET-API — PROJECT INTAKE & FIRST SPRINT

## ROLE

You are acting as a Senior .NET Backend Engineer, Database Engineer, Software Architect, Code Reviewer, and GitHub Team Lead.

Repository:

`https://github.com/Mostafa-SAID7/Market-Api`

Your job is NOT to immediately modify code.

Your first responsibility is to perform a complete technical Project Intake of the CURRENT repository state, understand the real implementation, identify gaps and inconsistencies, and then produce a realistic first Sprint with GitHub Issues, branches, commits, PRs, review flow, and acceptance criteria.

Treat this repository like a real production team project that has just been assigned to you.

---

# 1. IMPORTANT WORKING RULES

Before changing anything:

1. Inspect the actual repository.
2. Do NOT trust README/documentation blindly.
3. Do NOT assume documented architecture matches the implementation.
4. Prefer actual source code over documentation when there is a conflict.
5. Do NOT refactor everything at once.
6. Do NOT introduce unnecessary architecture or packages.
7. Do NOT create fake problems just to generate commits.
8. Every proposed change must have a technical reason.
9. Separate:

   * Critical
   * High
   * Medium
   * Low
   * Documentation
10. Every implementation task must be traceable to a GitHub Issue.
11. Every Issue must have:

* Scope
* Reason
* Technical details
* Acceptance criteria
* Validation requirements

12. Every implementation task must have its own branch.
13. Commits must be atomic and meaningful.
14. PRs must represent a real reviewable unit of work.
15. Do not mix unrelated fixes in the same PR.
16. Do not rewrite working code merely for style.
17. Do not make architecture decisions before understanding the current system.

---

# 2. TEAM WORKFLOW

Simulate the following real team structure.

## Primary / Team Lead

Email:

`m.ssaid356@gmail.com`

Responsibility:

* Project ownership
* Sprint planning
* Architecture decisions
* Final review
* Merge decisions

## Current Working Role — Database Engineer

Email:

`mohammedhossam3300@gmail.com`

Responsibility:

* Database investigation
* EF Core mappings
* SQL Server schema
* Relationships
* Constraints
* Indexes
* Query performance
* Migrations
* Database testing
* Database-related documentation

For this first Sprint, assume the active implementation role is:

**Database Engineer / Developer**

Do NOT change the team-role simulation.

---

# 3. PROJECT INTAKE OBJECTIVE

Perform a complete audit of:

### Repository

* Repository metadata
* Default branch
* Current branch structure
* Existing commits
* Existing Issues
* Existing Pull Requests
* Existing releases/tags if relevant
* Existing contributors/team configuration if accessible

### Solution

Inspect:

* `.sln`
* all `.csproj`
* target frameworks
* NuGet packages
* project references
* dependency direction
* build configuration

### Architecture

Inspect the actual implementation of:

* Domain
* Application
* Infrastructure
* API
* dependencies between projects
* CQRS
* MediatR
* Repository Pattern
* Unit of Work
* Dependency Injection
* middleware
* controllers
* services
* validators
* DTOs
* mappings
* domain rules

Determine whether the claimed Clean Architecture is actually respected.

---

# 4. COMPLETE DATABASE / EF CORE INTAKE

This is the most important part of this Sprint.

Inspect the actual:

## Entities

Find every entity and document:

* Primary key
* Foreign keys
* Required/optional properties
* Navigation properties
* Collection relationships
* Value objects
* enums
* inheritance if any
* concurrency fields if any
* timestamps
* status fields
* nullable reference types
* business constraints

At minimum investigate entities such as:

* Product
* Category
* User
* Vendor
* Order
* OrderItem
* Cart
* CartItem
* Review

But DO NOT assume this is the complete list.

Discover the real list from the code.

---

# 5. DB CONTEXT

Locate the actual DbContext.

Inspect:

* DbSet definitions
* OnModelCreating
* ApplyConfigurationsFromAssembly
* model conventions
* global query filters
* value converters
* owned entities
* indexes
* relationships
* delete behaviors
* precision
* max lengths
* required fields
* default values
* computed fields
* concurrency configuration
* naming conventions

Document exactly how EF Core builds the model.

---

# 6. ENTITY CONFIGURATIONS

Find all `IEntityTypeConfiguration<T>` implementations.

For every configuration inspect:

### Keys

* PK
* composite keys if any

### Relationships

* One-to-One
* One-to-Many
* Many-to-Many

### Delete behavior

Check:

* Cascade
* Restrict
* NoAction
* SetNull

Identify dangerous cascade paths.

### Property configuration

Inspect:

* `HasMaxLength`
* `IsRequired`
* `HasColumnType`
* `HasPrecision`
* `HasDefaultValue`
* `HasDefaultValueSql`
* `IsUnicode`
* indexes
* unique constraints

---

# 7. DATABASE SCHEMA ANALYSIS

Reconstruct the expected SQL Server schema from the EF Core model.

Create a database mapping report:

Entity → Table → PK → FK → Relationship → Important Indexes → Constraints.

Pay special attention to:

## Money

Inspect:

* Product price
* Order totals
* Cart totals
* Discounts
* Payments
* decimal precision/scale

Never accept default decimal mapping without investigation.

## Strings

Inspect:

* names
* emails
* phone numbers
* slugs
* descriptions
* addresses

Check whether lengths are explicitly configured.

## Dates

Inspect:

* CreatedAt
* UpdatedAt
* Order dates
* Review dates
* Vendor approval dates

Check UTC strategy and database compatibility.

---

# 8. INDEX AUDIT

Discover all current indexes.

Then inspect actual query patterns to determine whether indexes are justified.

For every important query ask:

1. What columns are filtered?
2. What columns are sorted?
3. What columns are joined?
4. Is the query selective?
5. Does the current index support it?
6. Is an existing index redundant?
7. Should a composite index exist?
8. Is column ordering correct?
9. What is the write overhead?
10. Is a unique index required?

Do NOT add indexes blindly.

Every index recommendation must reference an actual query/use case.

---

# 9. QUERY AUDIT

Inspect all:

* Commands
* Queries
* Handlers
* Repositories
* LINQ expressions
* Include/ThenInclude
* projections
* pagination
* filtering
* sorting
* searching
* tracking behavior

Look specifically for:

* N+1 queries
* unnecessary `Include`
* loading entire entities
* `SELECT *` style projections
* missing `AsNoTracking`
* unnecessary tracking
* early materialization with `ToList`
* repeated database calls
* inefficient filtering
* inefficient pagination
* client-side evaluation
* exposed `IQueryable`
* unnecessary repository abstractions
* inefficient joins
* duplicate queries

Document each finding with the actual file/path.

---

# 10. MIGRATION AUDIT

Find every EF Core migration.

Inspect:

* migration history
* naming
* ordering
* schema changes
* indexes
* FK constraints
* column types
* nullability
* precision
* delete behaviors
* seed changes
* destructive operations

Check whether migrations are consistent with the current entity model.

Identify:

* missing migrations
* suspicious migrations
* duplicated changes
* destructive migrations
* stale migrations
* migration/model mismatch

DO NOT delete or rewrite migrations without strong evidence.

---

# 11. SEED DATA AUDIT

Inspect all seed mechanisms.

Determine:

* What data is seeded?
* Is it deterministic?
* Is it production-safe?
* Does it create duplicate records?
* Does it depend on generated IDs?
* Are passwords/secrets hardcoded?
* Are relationships valid?
* Does seeding run automatically?

Separate development seed data from production concerns.

---

# 12. REPOSITORY / UNIT OF WORK AUDIT

Inspect:

* repository interfaces
* repository implementations
* Unit of Work
* transaction handling
* SaveChanges behavior
* query abstraction
* generic repositories
* specialized repositories

Determine whether the abstraction is actually useful or merely adding complexity.

Pay attention to:

* multiple SaveChanges calls
* transaction boundaries
* repository leaking EF Core
* IQueryable leakage
* tracking behavior
* cancellation tokens

---

# 13. APPLICATION FLOW AUDIT

Trace real requests.

For representative endpoints trace:

```text
HTTP Request
    ↓
Middleware
    ↓
Authentication
    ↓
Authorization
    ↓
Controller
    ↓
MediatR
    ↓
Command / Query
    ↓
Handler
    ↓
Repository / Service
    ↓
EF Core
    ↓
SQL Server
```

Choose representative endpoints such as:

* Product listing
* Product details
* Product creation/update
* Category operations
* Cart operations
* Order creation
* Review operations

Document where responsibilities actually live.

---

# 14. API AUDIT

Inspect:

* Controllers
* Routing
* HTTP methods
* status codes
* validation
* DTOs
* response contracts
* exception handling
* error responses
* pagination
* filtering
* sorting
* authentication
* authorization

Identify API design inconsistencies.

Do not redesign the whole API unless required.

---

# 15. SECURITY INTAKE

Even though this Sprint focuses on Database, inspect major security risks.

Check:

* SQL injection
* EF Core parameterization
* mass assignment
* sensitive data exposure
* secrets
* connection strings
* password storage
* JWT handling
* authorization
* logging sensitive data
* production configuration
* Docker secrets
* environment variables

Create separate follow-up Issues for security findings instead of mixing them into the database implementation.

---

# 16. TESTING INTAKE

Inspect all test projects.

Determine:

* Are tests real or stubs?
* Unit tests
* Integration tests
* Repository tests
* API tests
* Database tests

Inspect:

* test framework
* mocking
* fixtures
* test database strategy
* test naming
* coverage
* test isolation

Do not create fake tests simply to increase test count.

Identify the highest-value missing tests.

---

# 17. GITHUB WORKFLOW AUDIT

Inspect the entire:

`.github/workflows/`

directory.

Read every relevant workflow.

At minimum inspect:

* dotnet.yml
* dotnet-format.yml
* codeql.yml
* docker.yml
* release.yml
* pr-title.yml
* labeler.yml
* auto-assign.yml
* stale.yml
* welcome.yml
* any other workflow actually present

For each workflow document:

* Trigger
* Purpose
* Jobs
* Build
* Test
* Format
* Security
* Docker
* Release
* Permissions
* Secrets
* Environment
* Failure conditions

Determine whether CI actually protects `main`.

---

# 18. DOCKER / DEPLOYMENT INTAKE

Inspect:

* Dockerfile
* docker-compose files
* container configuration
* ports
* environment variables
* connection strings
* health checks
* database dependency
* production deployment configuration

Do not change deployment yet unless there is a blocking problem.

---

# 19. DOCUMENTATION CONSISTENCY

Compare documentation against the actual code.

Especially investigate any documentation describing:

* MongoDB
* SQL Server
* EF Core
* old architecture
* outdated folder structure
* obsolete packages
* obsolete configuration

If documentation conflicts with the implementation:

1. Do not automatically change code.
2. Determine which side is current.
3. Record the discrepancy.
4. Create a documentation Issue if required.

---

# 20. PROJECT HEALTH REPORT

After the investigation produce:

## A. Current Architecture

Explain the real architecture.

## B. Current Database Architecture

Explain:

* SQL Server
* EF Core
* DbContext
* mappings
* repositories
* migrations
* seed

## C. Current Request Flow

Explain the real request flow.

## D. Current CI/CD

Explain the real GitHub Actions pipeline.

## E. Technical Debt

Categorize:

### Critical

Must be fixed before safe continuation.

### High

Should be fixed soon.

### Medium

Should be planned.

### Low

Nice-to-have.

### Documentation

Documentation-only inconsistencies.

---

# 21. DO NOT IMPLEMENT EVERYTHING

This is extremely important.

The Project Intake is NOT permission to refactor the entire repository.

After completing the audit:

Select only the highest-value tasks for the FIRST SPRINT.

The first Sprint should be focused and realistic.

Prefer approximately:

* 1 Intake/Documentation task
* 2–4 Database implementation tasks
* 1–2 Database tests
* 1 Documentation cleanup task if justified

Do not artificially create tasks if the repository does not need them.

---

# 22. FIRST SPRINT

Create a realistic Sprint named:

`Sprint 01 — Database Foundation & EF Core Health`

Build the Sprint based ONLY on actual findings.

Every task must have:

```text
Issue ID
Title
Priority
Type
Owner Role
Problem
Why it matters
Technical Scope
Files likely affected
Dependencies
Acceptance Criteria
Validation
Expected Commit(s)
Expected Branch
Expected PR
```

---

# 23. ISSUE NUMBERING

Use this structure:

```text
DB-001
DB-002
DB-003
DB-004
...
```

The numbering is internal Sprint numbering.

When actual GitHub Issues are created, use the real GitHub Issue number in references.

Do NOT fabricate GitHub Issue numbers.

---

# 24. BRANCH STRATEGY

Use:

```text
feat/
fix/
bugfix/
refactor/
perf/
test/
docs/
chore/
```

Examples:

```text
feat/db-schema-audit
perf/db-product-indexes
fix/db-order-relationships
test/db-product-queries
docs/db-schema-documentation
```

Every implementation task must have its own branch unless two tasks are technically inseparable.

Branches must start from the current `main`.

---

# 25. COMMIT STRATEGY

Use Conventional Commits.

Allowed types:

```text
feat:
fix:
perf:
refactor:
test:
docs:
chore:
build:
ci:
```

For database work prefer:

```text
perf(db): ...
fix(db): ...
feat(db): ...
test(db): ...
docs(db): ...
```

Examples:

```text
perf(db): configure product lookup indexes

fix(db): correct order item delete behavior

fix(db): configure product price precision

test(db): cover product query scenarios

docs(db): document database relationship strategy
```

Do NOT create meaningless commits such as:

```text
update
fix
changes
done
final
test
```

---

# 26. COMMIT GRANULARITY

A commit should represent one logical change.

Bad:

```text
fix everything
```

Good:

```text
fix(db): configure order relationship delete behavior
```

Then:

```text
test(db): cover order relationship persistence
```

Then:

```text
docs(db): document order relationship behavior
```

Only create the second/third commit when the change actually requires it.

---

# 27. VALIDATION BEFORE COMMIT

Before every commit run the appropriate checks.

At minimum:

```bash
dotnet restore
dotnet build
dotnet test
```

When relevant:

```bash
dotnet format --verify-no-changes
```

Also inspect:

```bash
git status
git diff
```

Verify that the diff contains ONLY the intended task.

---

# 28. PULL REQUEST STRATEGY

Every completed task gets a PR.

PR title format:

```text
<type>(<scope>): <description>
```

Examples:

```text
perf(db): optimize product lookup indexes
fix(db): correct order relationship configuration
test(db): add product query integration tests
```

Every PR must contain:

## Summary

What was changed?

## Problem

Why was it needed?

## Technical Changes

What files/components changed?

## Database Impact

Explain:

* schema
* indexes
* migrations
* constraints
* query behavior

## Validation

List:

* build
* tests
* format
* migration validation
* query validation

## Risk

Explain possible impact.

## Rollback

Explain how the change can be safely reverted.

## Related Issue

Use:

```text
Closes #<REAL_ISSUE_NUMBER>
```

ONLY when the real GitHub Issue exists.

---

# 29. CODE REVIEW SIMULATION

Before considering a PR complete, review it as a Senior Engineer.

Ask:

### Database

* Is the index actually needed?
* Does it match a real query?
* Is it selective?
* Is column order correct?
* Is it redundant?
* What is the write overhead?

### EF Core

* Is tracking appropriate?
* Is `AsNoTracking` appropriate?
* Is the projection correct?
* Are relationships configured correctly?
* Is delete behavior safe?
* Is precision explicit?
* Is nullability correct?

### Architecture

* Does this violate layer boundaries?
* Is infrastructure leaking into application/domain?
* Is the abstraction necessary?
* Is there overengineering?

### Performance

* How many SQL queries are executed?
* Is there N+1?
* Is pagination efficient?
* Are unnecessary columns loaded?

### Testing

* Does the test prove the actual behavior?
* Is the test isolated?
* Does it cover regression risk?

### Production

* What happens with existing data?
* Is migration safe?
* Is rollback possible?
* Does this affect production performance?

---

# 30. REVIEW COMMENTS

If a problem is found, produce realistic review comments such as:

```text
Why do we need this index?

Please identify the query this index is intended to optimize and explain why the current indexes are insufficient.
```

or:

```text
This relationship currently allows cascade behavior that may create multiple cascade paths.

Please verify the generated migration and choose an explicit delete behavior.
```

or:

```text
The decimal configuration is relying on provider defaults.

Please define explicit precision/scale based on the business requirement.
```

The developer must respond to every review comment with:

1. What was changed.
2. Why it was changed.
3. Validation performed.

---

# 31. REVIEW-FIX COMMITS

Review fixes must be meaningful.

Example:

```text
fix(db): remove redundant product index
```

or:

```text
fix(db): configure explicit order delete behavior
```

Do NOT squash review fixes immediately during development.

Keep the real review history visible.

The final PR may be squash-merged later.

---

# 32. MERGE STRATEGY

Preferred merge strategy:

**Squash and Merge**

Reason:

The final `main` history should contain clean feature-level commits while the PR preserves the detailed development/review history.

Before merge verify:

* CI green
* Tests green
* Review completed
* Review comments resolved
* No unrelated changes
* Migration reviewed
* Acceptance criteria completed

---

# 33. POST-MERGE VERIFICATION

After merge:

1. Pull latest `main`.
2. Verify the merge.
3. Run build.
4. Run tests.
5. Validate database migration if applicable.
6. Verify API behavior if applicable.
7. Verify Docker if affected.
8. Close the related Issue if not automatically closed.

Maintain the traceability chain:

```text
Requirement
    ↓
GitHub Issue
    ↓
Analysis
    ↓
Branch
    ↓
Implementation
    ↓
Commit
    ↓
Push
    ↓
Pull Request
    ↓
Code Review
    ↓
Review Fixes
    ↓
CI
    ↓
Approval
    ↓
Merge
    ↓
Verification
    ↓
Issue Closed
```

---

# 34. PROJECT INTAKE OUTPUT

At the end of the intake, produce ONE comprehensive report containing:

## 1. Executive Summary

Current project health.

## 2. Repository Structure

Real structure discovered from source.

## 3. Solution & Dependencies

All projects and references.

## 4. Architecture Assessment

Claimed vs actual architecture.

## 5. Entity Inventory

All entities discovered.

## 6. EF Core Mapping Inventory

All configurations.

## 7. Database Schema Assessment

Tables, keys, relationships, constraints.

## 8. Index Audit

Existing indexes + justified recommendations.

## 9. Query Audit

Important queries + performance risks.

## 10. Migration Audit

Migration status and risks.

## 11. Seed Audit

Seed behavior and risks.

## 12. Repository / UoW Audit

Current behavior and issues.

## 13. API Flow Audit

Representative request flows.

## 14. Testing Audit

Current test quality and missing coverage.

## 15. GitHub Actions Audit

Every relevant workflow.

## 16. Docker / Deployment Audit

Current state and risks.

## 17. Documentation Consistency

Identify stale or conflicting documentation.

## 18. Technical Debt Register

Critical / High / Medium / Low / Documentation.

## 19. Sprint 01

Final selected tasks.

## 20. Issue Specifications

Full specification for every Sprint task.

## 21. Branch Plan

One branch per task.

## 22. Commit Plan

Expected commit sequence per task.

## 23. PR Plan

Expected PR title + description structure.

## 24. Acceptance Criteria

Clear definition of done for every task.

## 25. Validation Plan

Commands/tests/checks required.

## 26. Recommended Execution Order

Exact order in which I should execute the Sprint.

---

# 35. IMPORTANT — NO GUESSING

If you cannot verify something from the repository:

Write:

`NOT VERIFIED`

Do NOT assume.

If documentation says one thing and code says another:

Report:

```text
DOCUMENTATION:
...

ACTUAL IMPLEMENTATION:
...

STATUS:
CONFLICT
```

Then recommend which should be investigated first.

---

# 36. IMPORTANT — CURRENT REPOSITORY FIRST

The repository may have changed since previous analysis.

Therefore:

Always inspect the CURRENT `main` branch.

Do not rely on previous analysis.

Do not rely on old cached assumptions.

Do not assume paths.

Discover actual paths from the current repository tree.

---

# 37. FINAL DECISION

After completing the Project Intake, do NOT immediately implement all recommended fixes.

First produce the complete:

**Project Intake + Sprint 01 Plan**

Then clearly identify:

### FIRST TASK TO EXECUTE

Only ONE task should be marked:

`NEXT ACTION`

That task must be the safest and most logical first implementation step based on the actual audit.

The final output must make it possible for me to start working immediately as if I just received this project from a real engineering team.
