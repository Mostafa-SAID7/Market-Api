# COMPLETE WORKFLOW - From Issue to Merge

This document explains the FULL workflow that must be followed for every task, from business requirement to merged code.

---

## 🎯 THE WORKFLOW (16 Steps)

### STEP 1: BUSINESS REQUIREMENT → GITHUB ISSUE

**Who:** Team Lead (m.ssaid356@gmail.com)  
**What:** Create GitHub issue with clear objective

**Example: DB-001**
```
Title: Audit and normalize Market API SQL Server persistence layer

## Objective
Audit the current SQL Server database model and EF Core mappings
to identify schema inconsistencies, missing constraints, relationship
issues, indexing gaps, and performance risks.

## Scope
- Review DbContext
- Review entity configurations
- Review relationships
- Review keys
- Review indexes
- Review nullability
- Review decimal precision
- Review delete behaviors
- Review migrations
- Review seed data
- Review query patterns

## Acceptance Criteria
- [ ] All entities mapped correctly
- [ ] Relationships verified
- [ ] Required indexes identified
- [ ] Missing constraints documented
- [ ] Decimal/date/string column configuration reviewed
- [ ] Migration state verified
- [ ] Query performance risks documented
- [ ] Follow-up implementation issues created
```

**Key:** Issue describes WHAT, not HOW

---

### STEP 2: TICKET RECEIVED

**Who:** Mohammed Hossam (Database Engineer)  
**Action:** Read and understand the ticket

**Checklist:**
- [ ] Understand objective
- [ ] Review scope items
- [ ] Know acceptance criteria
- [ ] Identify dependencies

---

### STEP 3: ANALYSIS (No code yet!)

**Who:** Mohammed Hossam  
**Duration:** 1-3 days  
**Action:** Technical investigation without writing code

**Investigation Path:**
```
Domain Layer
    ↓ (find entities)
Entities Folder
    ↓ (review properties & types)
Infrastructure/Configurations/
    ↓ (review entity configurations)
DbContext
    ↓ (review DbSet definitions)
Repositories
    ↓ (review data access patterns)
Handlers/Queries
    ↓ (review query patterns)
Migrations/
    ↓ (audit migration history)
Documentation
    ↓ (check for inconsistencies)
```

**Output:** Technical findings document

---

### STEP 4: TECHNICAL DESIGN

**Who:** Mohammed Hossam + Team Lead discussion  
**Action:** Document findings and approach

**Find Format:**
```
FINDING #1: Product.Name has no explicit max length
Severity: High
Location: src/Market.Domain/Entities/Product.cs
Issue: String properties without MaxLength cause data inconsistency
Recommendation: Add MaxLength(255) in ProductConfiguration
```

**Categories:**
- Critical (breaks things)
- High (important improvements)
- Medium (should fix)
- Low (nice to have)
- Documentation (doc updates needed)

**Output:** 
- Findings report
- Sub-issues list (DB-002, DB-003, etc.)

---

### STEP 5: CREATE BRANCH

**Who:** Mohammed Hossam  
**Git Command:**
```bash
git checkout main
git pull origin main
git checkout -b feat/db-schema-audit
```

**Branch naming:**
- `feat/db-schema-audit` - For features
- `perf/db-product-indexes` - For performance
- `fix/cart-duplicate-items` - For bugs
- `refactor/db-repository-query` - For refactoring

---

### STEP 6: IMPLEMENTATION

**Who:** Mohammed Hossam  
**Action:** Write code based on findings

**Example changes:**
```csharp
// Before: No max length
public string Name { get; set; }

// After: Explicit max length
modelBuilder.Entity<Product>()
    .Property(p => p.Name)
    .HasMaxLength(255)
    .IsRequired();
```

---

### STEP 7: LOCAL VALIDATION (QUALITY GATES)

**Before committing, MUST run:**

```bash
# 1. Build
dotnet build
# ✅ Must pass with no errors

# 2. Test
dotnet test
# ✅ All tests must pass

# 3. Format check
dotnet format --verify-no-changes
# ✅ No formatting issues

# 4. Review changes
git diff
# ✅ Only related changes
# Question: Does this diff solve ONLY the issue in the ticket?
# If NO → split into separate commits
# If YES → proceed to commit
```

**If any fail:** Fix and re-run ALL gates

---

### STEP 8: COMMIT (ATOMIC)

**Who:** Mohammed Hossam  
**Format:**
```
<type>(<scope>): <subject>

<body>

<footer>
```

**Example:**
```
feat(db): add string max length constraints

Added explicit MaxLength configuration for all string columns
in Product entity to prevent data inconsistency.

- Product.Name: MaxLength(255)
- Product.SKU: MaxLength(50)
- Product.Description: MaxLength(2000)

Updated ProductConfiguration.cs with FluentAPI config.
Migration required: 202409221500_AddProductStringConstraints

Closes #12
```

**Types allowed:**
- `feat` - New feature
- `fix` - Bug fix
- `perf` - Performance
- `refactor` - Refactoring
- `test` - Tests
- `docs` - Documentation
- `chore` - Build/CI

**Scopes:**
- `db` - Database/EF Core
- `product` - Product domain
- `order` - Order domain
- `cart` - Cart domain
- etc.

**Rules:**
- Imperative present: "add" not "added"
- One logical change per commit
- Reference issue: `Closes #12` or `Fixes #25`

---

### STEP 9: PUSH BRANCH

**Who:** Mohammed Hossam  
**Command:**
```bash
git push -u origin feat/db-schema-audit
```

---

### STEP 10: CREATE PULL REQUEST

**Who:** Mohammed Hossam  
**Platform:** GitHub

**PR Title:** (same as commit message)
```
feat(db): add string max length constraints
```

**PR Description:**
```markdown
## Summary
Adds MaxLength constraints to all string properties in Product entity
to ensure data consistency across databases.

## Changes
- Added MaxLength(255) for Product.Name
- Added MaxLength(50) for Product.SKU
- Added MaxLength(2000) for Product.Description
- Updated ProductConfiguration with FluentAPI
- Created migration for constraints

## Validation
- ✅ dotnet build
- ✅ dotnet test
- ✅ dotnet format --verify-no-changes
- ✅ Migration verified locally

## Related Issue
Closes #12
```

---

### STEP 11: CODE REVIEW

**Who:** Team Lead (m.ssaid356@gmail.com)  
**Action:** Deep review, not just approval

**Reviewer asks questions:**
```
For indexes:
- Why this index?
- What query does it optimize?
- Is the index selective?
- Does it duplicate existing indexes?
- Write overhead acceptable?
- Migration impact on existing data?
- Index column order correct?

For configurations:
- Why this constraint?
- Is it enforced in code too?
- Breaking changes?
- Backward compatible?

For queries:
- N+1 patterns?
- AsNoTracking used correctly?
- Projection efficient?
- Pagination needed?
```

---

### STEP 12: ADDRESS REVIEW COMMENTS

**If reviewer says:**
```
"This index overlaps with existing composite index (CategoryId, IsActive)"
```

**Don't do:**
```bash
git commit -m "final"
git commit -m "fix review"
```

**Do this instead:**
```bash
# Make the fix
# Remove the redundant index

# Create focused commit
git commit -m "perf(db): remove redundant product index

Removed Product.CategoryId single index as it's covered
by existing composite index (CategoryId, IsActive).
Verified all queries use composite index."

# Push
git push

# Reply to reviewer:
"Removed redundant index. Verified existing composite index
covers all target queries."

# Then: Resolve conversation
```

---

### STEP 13: CI/CD VERIFICATION

**Who:** GitHub Actions (automated)  
**Process:**
```
Push code
    ↓
Build step
    ↓ (dotnet build)
Tests step
    ↓ (dotnet test)
Static Analysis
    ↓ (CodeQL, code quality)
Security check
    ↓ (dependency scanning)
```

**PR cannot merge if:**
- ❌ Build fails
- ❌ Tests fail
- ❌ Security issue
- ❌ Code quality fails

---

### STEP 14: APPROVAL

**Who:** Team Lead  
**Action:** 
- All comments addressed
- All tests passing
- CI/CD green
- Code reviewed

**Result:** ✅ Approved

---

### STEP 15: MERGE

**How:** Squash and merge

**Why:**
- Keeps main history clean
- One logical commit per feature
- Easier to revert if needed

**After merge:**
- Remote branch auto-deleted
- Main branch updated

---

### STEP 16: POST-MERGE VERIFICATION

**Who:** Mohammed Hossam  
**Commands:**
```bash
git checkout main
git pull origin main
```

**Verify:**
- [ ] Build succeeds
- [ ] Tests pass
- [ ] Migration applies (if DB change)
- [ ] No regressions
- [ ] API works
- [ ] Docker builds

**Then:**
- Close the GitHub issue
- Add summary comment

---

## 🔄 DAILY WORKFLOW EXAMPLE

### 09:00 - Ticket Intake
```
- Receive DB-001
- Read requirements
- Understand scope
```

### 09:15 - Understand Requirements
```
- Review acceptance criteria
- Ask clarifying questions
- Plan investigation
```

### 09:45 - Repository Investigation
```
- Locate DbContext
- Review entities
- Analyze configurations
- Check migrations
```

### 10:30 - Technical Design
```
- Document findings
- Plan sub-issues
- Identify patterns
```

### 11:00 - Create Branch
```bash
git checkout -b feat/db-schema-audit
```

### 11:15 - Implementation
```
- Make code changes
- Run dotnet build
- Run dotnet test
- Run dotnet format
```

### 13:00 - Testing
```
- Verify quality gates
- Review git diff
- Ensure atomic commits
```

### 14:00 - Break / Personal Time
(flexible based on schedule)

### 16:00 - Continue Work
```
- Finish implementation
- Address remaining items
```

### 17:00 - Self Review
```bash
git log main..HEAD --oneline
git diff main
```

### 17:30 - Commit
```bash
git add <files>
git commit -m "type(scope): description"
```

### 18:00 - Push & PR
```bash
git push -u origin feat/db-schema-audit
# Create PR on GitHub
```

### 18:30 - Self Review
```
- Review own PR
- Check description
- Verify acceptance criteria
```

### 19:00+ - Await Review
```
- Address review comments
- Run CI/CD again
- Resolve conversations
```

### Merge + Close
```
- Merge when approved
- Close issue
- Post-merge verification
```

---

## 📊 TRACEABILITY CHAIN

This is what makes it Enterprise-level:

```
Issue #12
  ↓ (DB-001: Audit database)
Branch: feat/db-schema-audit
  ↓ (Created from main)
Commits:
  - feat(db): add string constraints
  - perf(db): add product indexes
  - test(db): add constraint tests
  ↓
Pull Request #18
  ↓ (Code review conversation)
Review Comments
  ↓ (Address feedback)
Fix Commits
  ↓ (perf(db): remove redundant index)
CI/CD
  ↓ (All checks pass)
Approval
  ↓ (Team lead approves)
Merge Commit
  ↓ (Squash and merge)
Main Branch
  ↓ (Code in production ready)
Issue Closed
  ↓ (Marked as resolved)
```

**6 months later, someone asks:**
```
"Why do we have this index?"
```

**You can trace:**
```
Issue #12 discussion
  → PR #18 comments
    → Commit message explanation
      → Technical reasoning
        → Decision documented
```

This is **Enterprise traceability**.

---

## ✅ QUALITY GATES CHECKLIST

Before every commit:
- [ ] `dotnet build` passes
- [ ] `dotnet test` passes
- [ ] `dotnet format` clean
- [ ] `git diff` only related changes
- [ ] Commit message descriptive
- [ ] One logical change only

Before PR merge:
- [ ] PR approved
- [ ] CI/CD all green
- [ ] No merge conflicts
- [ ] Squash and merge strategy

After merge:
- [ ] Main builds
- [ ] Tests pass
- [ ] No regressions
- [ ] Issue closed
- [ ] Documentation updated

---

## 🚫 WHAT NOT TO DO

**❌ Don't:**
- Push directly to main
- Force push to main
- Mix multiple features in one branch
- Commit without quality gates
- Use vague messages ("fix", "update", "final")
- Skip code review
- Merge without CI/CD passing

**✅ Do:**
- Always branch from main
- Keep branches focused
- Atomic commits
- Descriptive messages
- Code reviews with substance
- CI/CD must pass
- Traceability always

---

**This workflow is what separates professional development from random commits.**

When you follow this, your GitHub profile shows you understand **Team Development Lifecycle**.

See:
- `.agent/git.md` for git commands
- `.agent/status.md` for current phase
- `.agent/database/phase-1/` for DB-001 scope
