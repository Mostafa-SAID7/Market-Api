using MediatR;
using Market.API.Controllers;
using Market.Application.Features.Products;
using Market.Application.Features.Products.Commands;
using Market.Application.Features.Products.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Market.API.Tests.Controllers
{
    /// <summary>
    /// Integration tests for ProductsController API contract.
    /// Verifies correct HTTP responses, status codes, and response shapes.
    /// </summary>
    public class ProductsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly Mock<ILogger<ProductsController>> _loggerMock = new();
        private ProductsController _controller = null!;

        public ProductsControllerTests()
        {
            _controller = new ProductsController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkWithProducts()
        {
            // Arrange
            var products = new List<ProductResponse>
            {
                new() { Id = 1, Name = "Product 1", Price = 10m, CategoryId = 1, VendorId = 1 },
                new() { Id = 2, Name = "Product 2", Price = 20m, CategoryId = 1, VendorId = 1 }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var returnedProducts = Assert.IsAssignableFrom<List<ProductResponse>>(okResult.Value);
            Assert.Equal(2, returnedProducts.Count);
        }

        [Fact]
        public async Task Get_ReturnsOkWithEmptyList_WhenNoProducts()
        {
            // Arrange
            var emptyList = new List<ProductResponse>();
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(emptyList);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var returnedProducts = Assert.IsAssignableFrom<List<ProductResponse>>(okResult.Value);
            Assert.Empty(returnedProducts);
        }

        [Fact]
        public async Task GetById_ReturnsOkWithProduct_WhenExists()
        {
            // Arrange
            var product = new ProductResponse
            {
                Id = 1,
                Name = "Test Product",
                Price = 99.99m,
                CategoryId = 1,
                VendorId = 1
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var returnedProduct = Assert.IsType<ProductResponse>(okResult.Value);
            Assert.Equal(1, returnedProduct.Id);
            Assert.Equal("Test Product", returnedProduct.Name);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductResponse?)null);

            // Act
            var result = await _controller.Get(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WhenSuccessful()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "New Product",
                Description = "Test",
                Price = 50m,
                CategoryId = 1
            };

            var createdProduct = new ProductResponse
            {
                Id = 1,
                Name = "New Product",
                Price = 50m,
                CategoryId = 1,
                VendorId = 1
            };

            _mediatorMock
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdProduct);

            // Act
            var result = await _controller.Create(command);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(nameof(ProductsController.Get), createdResult.ActionName);
            Assert.Equal(createdProduct.Id, ((ProductResponse)createdResult.Value!).Id);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenArgumentExceptionThrown()
        {
            // Arrange
            var command = new CreateProductCommand { Name = "", Price = 0 };

            _mediatorMock
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("Invalid product data"));

            // Act
            var result = await _controller.Create(command);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Update_ReturnsOkWithUpdatedProduct_WhenSuccessful()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = 1,
                Name = "Updated Product",
                Price = 75m
            };

            var updatedProduct = new ProductResponse
            {
                Id = 1,
                Name = "Updated Product",
                Price = 75m,
                CategoryId = 1,
                VendorId = 1
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedProduct);

            // Act
            var result = await _controller.Update(1, command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var returnedProduct = Assert.IsType<ProductResponse>(okResult.Value);
            Assert.Equal("Updated Product", returnedProduct.Name);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenProductNotFound()
        {
            // Arrange
            var command = new UpdateProductCommand { Id = 999, Name = "Test" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException("Product not found"));

            // Act
            var result = await _controller.Update(999, command);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenProductNotFound()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException("Product not found"));

            // Act
            var result = await _controller.Delete(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}
