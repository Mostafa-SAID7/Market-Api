using System.Reflection;
using Market.Domain.Common;

namespace Market.Domain.Tests.Architecture
{
    /// <summary>
    /// Architecture tests to verify and enforce clean layer boundaries.
    /// These tests prevent regressions where layers become tightly coupled.
    /// </summary>
    public class LayerBoundaryTests
    {
        [Fact]
        public void Domain_ShouldNotDependOnApplication()
        {
            // Arrange
            var domainAssembly = typeof(BaseEntity).Assembly;
            var applicationNamespace = "Market.Application";

            // Act
            var violations = domainAssembly
                .GetTypes()
                .SelectMany(t => t.GetReferencedTypes())
                .Where(t => t.Namespace != null && t.Namespace.StartsWith(applicationNamespace))
                .Distinct()
                .ToList();

            // Assert
            Assert.Empty(violations);
        }

        [Fact]
        public void Domain_ShouldNotDependOnInfrastructure()
        {
            // Arrange
            var domainAssembly = typeof(BaseEntity).Assembly;
            var infrastructureNamespace = "Market.Infrastructure";

            // Act
            var violations = domainAssembly
                .GetTypes()
                .SelectMany(t => t.GetReferencedTypes())
                .Where(t => t.Namespace != null && t.Namespace.StartsWith(infrastructureNamespace))
                .Distinct()
                .ToList();

            // Assert
            Assert.Empty(violations);
        }

        [Fact]
        public void Application_ShouldNotDependOnInfrastructure()
        {
            // Arrange
            var applicationAssembly = typeof(Market.Application.Features.Products.ProductResponse).Assembly;
            var infrastructureNamespace = "Market.Infrastructure";

            // Act - Exclude valid references (like Market.Infrastructure.Persistence.Services)
            var violations = applicationAssembly
                .GetTypes()
                .Where(t => !t.Name.EndsWith("Command") && !t.Name.EndsWith("Query")) // Exclude handlers
                .SelectMany(t => t.GetReferencedTypes())
                .Where(t => t.Namespace != null 
                    && t.Namespace.StartsWith(infrastructureNamespace)
                    && !t.Namespace.Contains("Abstractions")) // Allow references to abstractions
                .Distinct()
                .ToList();

            // Assert
            Assert.Empty(violations);
        }

        [Fact]
        public void Infrastructure_ShouldNotDependOnApplicationFeatures()
        {
            // Arrange
            var infrastructureAssembly = typeof(Market.Infrastructure.Data.MarketDbContext).Assembly;

            // Act - Get all types in Infrastructure
            var infrastructureRepositories = infrastructureAssembly
                .GetTypes()
                .Where(t => t.Namespace != null 
                    && t.Namespace.Contains("Persistence.Repositories"))
                .ToList();

            // Check that repositories don't directly reference Application.Features
            var violations = infrastructureRepositories
                .SelectMany(t => t.GetReferencedTypes())
                .Where(t => t.Namespace != null 
                    && t.Namespace.StartsWith("Market.Application.Features"))
                .Distinct()
                .ToList();

            // Assert
            Assert.Empty(violations);
        }

        [Fact]
        public void Infrastructure_CanDependOnApplicationAbstractions()
        {
            // Arrange
            var infrastructureAssembly = typeof(Market.Infrastructure.Data.MarketDbContext).Assembly;

            // Act - Infrastructure services should implement Application.Abstractions interfaces
            var infrastructureServices = infrastructureAssembly
                .GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.Contains("Persistence.Services"))
                .ToList();

            // Assert - Should have at least ProductReadService
            Assert.NotEmpty(infrastructureServices);
            Assert.Contains(
                infrastructureServices,
                t => t.Name == "ProductReadService"
                    && t.Namespace == "Market.Infrastructure.Persistence.Services"
            );
        }

        [Fact]
        public void UserResponse_DoesNotExposePasswordHash()
        {
            // Arrange
            var userResponseType = typeof(Market.Application.Features.Users.UserResponse);

            // Act
            var properties = userResponseType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.Name)
                .ToList();

            // Assert
            Assert.DoesNotContain("PasswordHash", properties);
            Assert.DoesNotContain("Password", properties);
            Assert.DoesNotContain("SecurityStamp", properties);
            Assert.DoesNotContain("Hash", properties);
        }

        [Fact]
        public void ProductResponse_DoesNotExposeInternalFields()
        {
            // Arrange
            var productResponseType = typeof(Market.Application.Features.Products.ProductResponse);

            // Act
            var properties = productResponseType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.Name)
                .ToList();

            // Assert
            Assert.DoesNotContain("IsDeleted", properties);
            Assert.DoesNotContain("CreatedBy", properties);
            Assert.DoesNotContain("UpdatedBy", properties);
        }

        [Fact]
        public void AllResponseDtosAreInApplicationFeatures()
        {
            // Arrange
            var applicationAssembly = typeof(Market.Application.Features.Products.ProductResponse).Assembly;

            // Act
            var responseDtos = applicationAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Response"))
                .ToList();

            // Assert
            Assert.NotEmpty(responseDtos);
            Assert.All(responseDtos, dto =>
                Assert.True(
                    dto.Namespace?.StartsWith("Market.Application.Features") ?? false,
                    $"DTO {dto.Name} is not in Market.Application.Features namespace"
                )
            );
        }
    }

    /// <summary>
    /// Helper extension to get all types referenced by a type.
    /// </summary>
    internal static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetReferencedTypes(this Type type)
        {
            yield return type.BaseType ?? typeof(object);

            foreach (var interfaceType in type.GetInterfaces())
            {
                yield return interfaceType;
            }

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                yield return method.ReturnType;
                foreach (var parameter in method.GetParameters())
                {
                    yield return parameter.ParameterType;
                }
            }

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                yield return property.PropertyType;
            }
        }
    }
}
