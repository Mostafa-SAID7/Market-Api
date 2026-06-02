# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0] - 2026-06-03 - MediatR & Full Feature Implementation

### Added - MediatR Pattern (Complete Implementation)
- MediatR command/query pattern for all 7 entities with dedicated handlers
- Response DTOs for Products, Categories, Users, Vendors, Orders, Carts, Reviews
- CQRS-style architecture with Commands (Create, Update, Delete, Approve) and Queries (GetAll, GetById, Filtered)
- Automatic handler registration via MediatRConfiguration
- Special operations: ApproveVendor, AddToCart, RemoveFromCart, ClearCart, GetProductsByCategory, GetReviewsByProductId

### Added - Validation Framework
- Comprehensive IValidator interface with ValidationResult and ValidationError
- Entity validators: ProductValidator, CategoryValidator, UserValidator, VendorValidator, OrderValidator, CartValidator, ReviewValidator
- Validation rules: required fields, length constraints, format validation (email, URL, slug, phone), numeric ranges
- Middleware for validation exception handling
- Scoped DI registration via ValidatorConfiguration

### Added - Service Layer
- Service layer for all 7 entities with logging integration
- Services: ProductService, CategoryService, UserService, VendorService, OrderService, CartService, ReviewService
- Service interfaces for testability
- UnitOfWork pattern for coordinated repository access across multiple entities
- ServiceConfiguration for automatic DI registration

### Added - API Endpoints (Complete REST)
- CRUD endpoints for all 7 entities (Create, Read, Update, Delete)
- Special operations: Approve Vendor, Add to Cart, Remove from Cart, Clear Cart
- Proper HTTP status codes (200, 201, 400, 404)
- Error responses with validation details

### Added - Database Layer
- MongoDB integration with ObjectId serialization
- BaseEntity with Id, CreatedAt, UpdatedAt properties
- Repository pattern with generic and specific implementations
- Unit of Work coordination
- Index creation with duplicate handling
- BSON attribute configuration for proper serialization

### Fixed - Critical Bugs
- Entity property mappings in all DTOs and Commands
- Order: CustomerId (not UserId), Items (not ProductIds), TotalPrice (not TotalAmount), OrderStatus enum
- Review: CustomerId (not UserId), RatingValue (not Rating), VendorId, Title properties
- User: PhoneNumber (not Phone), removed non-existent Address
- Vendor: StoreName (not Name), IsApproved boolean (not Status string), TotalReviews, location fields
- Cart query implementation using GetAllAsync() with filtering
- Program.cs using statements for configurations

### Changed - Documentation
- Updated API.md to focus only on endpoint reference (removed architecture details)
- Consolidated architecture documentation in ARCHITECTURE.md
- Updated CONTRIBUTING.md with MediatR patterns
- Enhanced README with complete feature list

## [1.0.0] - 2026-04-21

### Added
- Initial ASP.NET Core 9 with MongoDB integration
- Repository pattern implementation
- Product CRUD operations
- Swagger/OpenAPI documentation  
- Docker and Docker Compose support

### Features
- GET /api/products - Get all products
- GET /api/products/{id} - Get product by ID
- GET /api/products/GetByPriceRange/{minPrice}/{maxPrice} - Filter by price
- POST /api/products - Create product
- PUT /api/products/{id} - Update product
- DELETE /api/products/{id} - Delete product

### Technical Stack
- ASP.NET Core 9, MongoDB 8.2, Docker, Swagger/OpenAPI
