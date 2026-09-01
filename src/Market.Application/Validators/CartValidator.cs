using Market.Domain.Entities;

namespace Market.Application.Validators
{
    /// <summary>
    /// Validator for Cart entity
    /// </summary>
    public class CartValidator : IValidator<Cart>
    {
        public ValidationResult Validate(Cart entity)
        {
            var result = new ValidationResult();

            // UserId validation
            if (entity.UserId <= 0)
                result.AddError(nameof(Cart.UserId), "User ID is required");

            // Items validation
            if (entity.Items != null && entity.Items.Count > 0)
            {
                foreach (var item in entity.Items)
                {
                    if (item.ProductId <= 0)
                        result.AddError(nameof(Cart.Items), "Product ID is required for all cart items");

                    if (item.Quantity <= 0)
                        result.AddError(nameof(Cart.Items), "Quantity must be greater than 0");
                    else if (item.Quantity > 999)
                        result.AddError(nameof(Cart.Items), "Quantity cannot exceed 999");

                    if (item.Price < 0)
                        result.AddError(nameof(Cart.Items), "Price cannot be negative");

                    if (string.IsNullOrWhiteSpace(item.ProductName))
                        result.AddError(nameof(Cart.Items), "Product name is required");
                }
            }

            return result;
        }
    }
}


