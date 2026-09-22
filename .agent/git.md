# GIT WORKFLOW & COMMIT STANDARDS

## BRANCH NAMING

`<type>/<scope>-<description>`

### Types
- `feat/` - New features
- `fix/` - Bug fixes
- `perf/` - Performance improvements
- `refactor/` - Refactoring
- `test/` - Tests
- `docs/` - Documentation
- `chore/` - Build/CI/dependencies

### Examples
```bash
git checkout -b feat/db-schema-audit
git checkout -b perf/db-product-indexes
git checkout -b fix/cart-duplicate-items
```

---

## BASIC WORKFLOW

### Start
```bash
git checkout main
git pull origin main
git checkout -b feat/db-xxx
```

### Develop
```bash
dotnet build
dotnet test
dotnet format --verify-no-changes
git diff
```

### Commit (ATOMIC - one logical change)
```bash
git add <specific-files>
git commit -m "type(scope): description"
git push -u origin feat/db-xxx
```

---

## COMMIT MESSAGE FORMAT

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types
- `feat` - New feature
- `fix` - Bug fix
- `perf` - Performance
- `refactor` - Refactoring
- `test` - Tests
- `docs` - Documentation
- `chore` - Build/CI
- `build` - Build system
- `ci` - CI/CD config

### Scopes
- `db` - Database/EF Core
- `product` - Product domain
- `order` - Order domain
- `cart` - Cart domain
- `user` - User domain
- `vendor` - Vendor domain
- `api` - API/Controller
- `middleware` - Middleware
- `infra` - Infrastructure

### Subject
- Imperative present tense: "add" not "added"
- No capital first letter
- No period at end
- Max 50 chars

### Body
- Explain motivation/change
- Reference issue: `Closes #25`, `Fixes #12`
- Wrap at 72 chars

---

## COMMIT EXAMPLES

### Feature
```
feat(db): add product lookup indexes

Added indexes for common query patterns:
- Index on CategoryId for filtering
- Index on SKU for lookup
- Composite (CategoryId, IsActive)

Migration: 202409221430_AddProductIndexes
Closes #12
```

### Performance
```
perf(product): optimize category query

Changed to use AsNoTracking and projection.
Query time: 450ms → 120ms

Closes #18
```

### Bug Fix
```
fix(cart): prevent duplicate items

Added unique constraint on (CartId, ProductId)
and validation in CartService.

Fixes #22
```

### Database Config
```
perf(db): configure decimal precision

Product.Price: Precision(18, 2), NotNull, Default(0)
Prevents rounding issues in financial calculations.

Related-To: #25
```

### Tests
```
test(db): add repository query coverage

Added tests for:
- GetByCategory with pagination
- GetBySKU with AsNoTracking
- GetAllActive with index verification

Coverage: 68% → 84%

Closes #30
```

---

## QUALITY GATES (Before Commit)

✅ Must pass ALL:
- `dotnet build` succeeds
- `dotnet test` passes
- `dotnet format --verify-no-changes` clean
- `git diff` contains ONLY related changes
- Commit is ATOMIC (one concern)

---

## PUSH & PR

```bash
# Push
git push -u origin feat/db-xxx

# View branch history
git log main..HEAD --oneline

# If need to clean up commits
git rebase -i main
# (pick, squash, reword as needed)

# Force push after rebase (use carefully)
git push --force-with-lease origin feat/db-xxx
```

---

## SAFETY RULES

❌ DON'T:
- Push directly to main
- Force push to main
- Mix multiple features in one branch
- Commit without quality gates
- Use vague messages ("fix", "update", "final")

✅ DO:
- Always branch from main
- Keep branch focused (one concern)
- Atomic, well-described commits
- Test locally before push
- Reference issues in commits

---

## USEFUL COMMANDS

```bash
# View your commits
git log main..HEAD --oneline

# View specific change
git diff main src/file.cs

# Stage specific file
git add src/file.cs

# See what will be committed
git diff --cached

# Amend last commit (only if not pushed)
git commit --amend

# Stash changes temporarily
git stash
git stash pop

# View all branches
git branch -a

# Delete local branch
git branch -d feat/db-xxx

# Delete remote branch
git push origin --delete feat/db-xxx
```
