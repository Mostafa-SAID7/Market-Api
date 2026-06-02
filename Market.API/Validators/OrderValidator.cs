using Market.API.Models.Entities;

namespace Market.API.Validators
{
    /// <summary>
    /// Validator for Order entity
    /// </summary>
    public class OrderValidator : IValidator<Order>
    {
        public ValidationResult Validate(Order entity)
        {
            var result = new ValidationResult();

            // CustomerId validation
            if (string.IsNullOrWhiteSpace(entity.CustomerId))
                result.AddError(nameof(Order.CustomerId), "Customer ID is required");

            // OrderNumber validation
            if (string.IsNullOrWhiteSpace(entity.OrderNumber))
                result.AddError(nameof(Order.OrderNumber), "Order number is required");

            // Items validation
            if (entity.Items == null || entity.Items.Count == 0)
                result.AddError(nameof(Order.Items), "Order must have at least one item");
            else
            {
                foreach (var item in entity.Items)
                {
                    if (string.IsNullOrWhiteSpace(item.ProductId))
                        result.AddError(nameof(Order.Items), "Product ID is required for all items");
                    if (item.Quantity <= 0)
                        result.AddError(nameof(Order.Items), "Quantity must be greater than 0 for all items");
                    if (item.Price < 0)
                        result.AddError(nameof(Order.Items), "Price cannot be negative for items");
                }
            }

            // Prices validation
            if (entity.SubTotal < 0)
                result.AddError(nameof(Order.SubTotal), "Subtotal cannot be negative");

            if (entity.ShippingCost < 0)
                result.AddError(nameof(Order.ShippingCost), "Shipping cost cannot be negative");

            if (entity.Tax < 0)
                result.AddError(nameof(Order.Tax), "Tax cannot be negative");

            if (entity.TotalPrice < 0)
                result.AddError(nameof(Order.TotalPrice), "Total price cannot be negative");

            // ShippingAddress validation
            if (string.IsNullOrWhiteSpace(entity.ShippingAddress))
                result.AddError(nameof(Order.ShippingAddress), "Shipping address is required");
            else if (entity.ShippingAddress.Length < 10)
                result.AddError(nameof(Order.ShippingAddress), "Shipping address must be at least 10 characters");
            else if (entity.ShippingAddress.Length > 500)
                result.AddError(nameof(Order.ShippingAddress), "Shipping address cannot exceed 500 characters");

            return result;
        }
    }
}
