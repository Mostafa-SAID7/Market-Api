# ACTIVE ISSUES & BRANCHES

## PHASE 1: Database Audit

### DB-001: Audit SQL Server Persistence Layer

**Status:** In Analysis  
**Branch:** `feat/db-schema-audit`  
**Assignee:** Mohammed Hossam (mohammedhossam3300@gmail.com)  
**Created:** 2026-09-22

**Scope:**
- [ ] DbContext & entities
- [ ] Configurations (FluentAPI)
- [ ] Relationships & cardinality
- [ ] Primary & foreign keys
- [ ] Indexes (existing & missing)
- [ ] Nullability & precision
- [ ] Delete behaviors
- [ ] Migrations audit
- [ ] Seed data
- [ ] Query patterns
- [ ] Documentation consistency

**Sub-Issues (To Create):**
- DB-002: Configure missing indexes
- DB-003: Fix decimal precision
- DB-004: Verify cascade behaviors
- DB-005: Optimize queries
- DB-006: Audit migrations
- DB-007: Update documentation

**Reference:**
See `.agent/database/phase-1/README.md` for full scope and investigation guidelines.

---

## COMPLETED ISSUES

(None yet)

---

## UPCOMING PHASES

**Phase 2:** Database Performance  
**Phase 3:** EF Core Engineering  
**Phase 4:** Testing  
**Phase 5:** Security  
**Phase 6:** DevOps  
**Phase 7:** Production Readiness  
**Phase 8:** System Design

---

## GIT REFERENCE

**Configured Git Account:**
```
Name: Mohammed Hossam
Email: mohammedhossam3300@gmail.com
```

**Standard Commands:**
```bash
# Create branch
git checkout main
git pull origin main
git checkout -b feat/db-schema-audit

# Commit (atomic)
git add <files>
git commit -m "type(scope): description"

# Push & PR
git push -u origin feat/db-schema-audit
# Then create PR on GitHub
```

**See:** `.agent/git.md` for detailed git workflow

---

**Last Updated:** 2026-09-22  
**Status:** Ready for Phase 1 Investigation
