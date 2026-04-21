# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Copy-to-clipboard functionality for all code blocks with visual feedback
- Explicit `/index.html` routing for consistent home page navigation
- Root path (`/`) now redirects to `/index.html` automatically
- Custom favicon with shopping cart design (gold gradient with glow)
- Professional logo with cinematic gold gradient and glow effects
- Custom scrollbar styling with brown/gold theme for Chrome and Firefox
- Active navigation link highlighting with `.active` class
- Responsive design improvements for mobile devices (768px and 480px breakpoints)
- Proper 404 page routing using MapFallback middleware
- Complete wwwroot static pages (index.html, docs.html, 404.html)
- Microsoft-inspired cinematic dark brown theme (#3d2817, #5c3d2e, #8b6f47, #d4af37)
- Interactive API status indicator on home page
- Smooth scroll animations and parallax effects
- Comprehensive API documentation with code examples (JavaScript, cURL)

### Changed
- Updated all home links from `/` to `/index.html` across all pages
- Simplified logo and favicon to show only shopping cart (removed API brackets)
- Reduced navigation header height (0.75rem padding) for more compact design
- Improved button contrast for WCAG AA accessibility compliance
- Enhanced code block styling with proper syntax highlighting
- Optimized middleware order for proper static file serving
- Updated link styles globally - removed underlines, added cursor pointer
- Improved responsive behavior for navigation on mobile
- Enhanced card animations with intersection observer
- Better error handling in 404 page with helpful links and common mistakes section

### Fixed
- 404 page now properly displays for wrong paths without interfering with static files
- Home page routing works correctly when clicking navigation links
- Navigation links properly show active state based on current path
- All links are now clickable without text decoration underlines
- Copy button on code blocks has proper contrast (white text with shadow)
- Scrollbar styling consistent across browsers (Chrome/Firefox)
- Mobile navigation layout improved with better spacing
- Static files (including index.html) are served correctly before fallback route
- JavaScript active link detection handles both `/` and `/index.html` as home page

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
