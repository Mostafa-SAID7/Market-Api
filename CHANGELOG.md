## [2.0.1](https://github.com/Mostafa-SAID7/Market-Api/compare/v2.0.0...v2.0.1) (2026-09-01)


### Bug Fixes

* Correct publish path in dotnet.yml workflow to use src/Market.API ([5152760](https://github.com/Mostafa-SAID7/Market-Api/commit/515276006f24c5c626ac6e86db48325c9adb80f7))

# [2.0.0](https://github.com/Mostafa-SAID7/Market-Api/compare/v1.0.0...v2.0.0) (2026-09-01)


### Features

* Complete Clean Architecture refactor with SOLID principles and pure CQRS ([544fcf4](https://github.com/Mostafa-SAID7/Market-Api/commit/544fcf46e126cef701baa5b9fe479fdbcdbb9407))


### BREAKING CHANGES

* Migrated from MongoDB to SQL Server. Update connection strings and database setup.

- Implement strict 4-layer architecture (Domain → Application → Infrastructure → API)
- Apply SOLID principles throughout codebase
- Implement pure CQRS with MediatR
- Migrate to SQL Server with Entity Framework Core 9.0.19
- Reorganize Infrastructure layer (Data/ for DbContext, Persistence/ for repositories)
- Update all namespaces to reflect new architecture
- Fix package dependency versions (DependencyInjection.Abstractions 10.0.0)
- Optimize Docker build to only include source projects
- Add comprehensive CHANGELOG documentation

# 1.0.0 (2026-08-31)


### Bug Fixes

* add security headers to fix CodeQL X-Frame-Options alert ([30872ea](https://github.com/Mostafa-SAID7/Market-Api/commit/30872eaeda958123c127fc0e47259f31a6ad6c8a))
* add X-Frame-Options and security headers to web.config for CodeQL ([f7f7ea0](https://github.com/Mostafa-SAID7/Market-Api/commit/f7f7ea03c8db790f2baaa81dd42e25c2cc6ad289))
* change release-please type to simple ([e194990](https://github.com/Mostafa-SAID7/Market-Api/commit/e194990ffe2e66c6d0c8f4d4cb4ed4a048c35198))
* Correct 404 routing and enhance code block styling ([eb8c0ab](https://github.com/Mostafa-SAID7/Market-Api/commit/eb8c0ab08961267bb393bd54a385a93d49c01402))
* correct middleware order to properly serve static files ([114d93b](https://github.com/Mostafa-SAID7/Market-Api/commit/114d93bc739129c455a999a63109d8624a66e2a0))
* resolve EntityFramework package downgrade error by aligning versions ([8bdf933](https://github.com/Mostafa-SAID7/Market-Api/commit/8bdf933e9934d08a9d2df0c91ee2cd612f17e705))


### Features

* Add favicon and logo, improve UI styling with cleaner borders ([2848b4d](https://github.com/Mostafa-SAID7/Market-Api/commit/2848b4d527c1d73c256e22948d2a5aa86383fd2c))
* Complete UI/UX improvements and 404 routing fix ([35d2fdf](https://github.com/Mostafa-SAID7/Market-Api/commit/35d2fdf6a696f1e0bfd1317d293e079dfccf52c3))
* Improve button contrast and ensure proper routing ([751b923](https://github.com/Mostafa-SAID7/Market-Api/commit/751b9234e7a11518004dd3cec3ecf24db1497de9)), closes [#ffffff](https://github.com/Mostafa-SAID7/Market-Api/issues/ffffff)
