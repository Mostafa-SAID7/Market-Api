# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Custom favicon with shopping cart and API brackets design
- Professional logo with cinematic gold gradient and glow effects
- Custom scrollbar styling with brown/gold theme
- Active navigation link highlighting
- Responsive design improvements for mobile devices (768px and 480px breakpoints)
- Proper 404 page routing using MapFallback
- Complete wwwroot static pages (index, docs, 404)
- Microsoft-inspired cinematic dark brown theme

### Changed
- Reduced navigation header height for more compact design
- Improved border styling with subtle rgba opacity
- Enhanced responsive behavior for navigation on mobile
- Updated all link styles to remove default underlines
- Optimized logo and favicon sizes for different screen sizes

### Fixed
- 404 page now properly displays for wrong paths
- Navigation links properly show active state
- All links no longer have text decoration underlines
- Scrollbar styling consistent across browsers (Chrome/Firefox)
- Mobile navigation layout improved with better spacing

## [1.0.0] - 2026-04-21

### Added
- Initial release of Market API
- ASP.NET Core 9 with MongoDB integration
- Repository pattern implementation
- Product CRUD operations
- Swagger/OpenAPI documentation
- Docker and Docker Compose support
- RESTful API endpoints for product management
- Price range filtering functionality

### Features
- GET /api/products - Retrieve all products
- GET /api/products/{id} - Get product by ID
- GET /api/products/GetByPriceRange/{minPrice}/{maxPrice} - Filter by price
- POST /api/products - Create new product
- PUT /api/products/{id} - Update existing product
- DELETE /api/products/{id} - Delete product

### Technical Stack
- ASP.NET Core 9
- MongoDB 8.2
- Docker & Docker Compose
- Swagger/OpenAPI
- Repository Pattern
- Generic Repository Implementation
