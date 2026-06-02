using Market.API.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace Market.API.Models.Entities
{
    /// <summary>
    /// Shopping cart entity
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Cart : BaseEntity
    {
        public string UserId { get; set; } = string.Empty; // Reference to User
        
        public List<CartItem> Items { get; set; } = new();
        
        public decimal SubTotal => Items.Sum(x => x.SubTotal);
        public int TotalItems => Items.Sum(x => x.Quantity);

        /// <summary>
        /// Add item to cart
        /// </summary>
        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(x => x.ProductId == item.ProductId && x.VendorId == item.VendorId);
            
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }

            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Remove item from cart
        /// </summary>
        public void RemoveItem(string productId)
        {
            Items.RemoveAll(x => x.ProductId == productId);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Update item quantity
        /// </summary>
        public void UpdateItemQuantity(string productId, int quantity)
        {
            var item = Items.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    RemoveItem(productId);
                }
                else
                {
                    item.Quantity = quantity;
                    UpdatedAt = DateTime.UtcNow;
                }
            }
        }

        /// <summary>
        /// Clear cart
        /// </summary>
        public void Clear()
        {
            Items.Clear();
            UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Cart item - product in cart
    /// </summary>
    [BsonIgnoreExtraElements]
    public class CartItem
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string VendorId { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        public decimal SubTotal => Price * Quantity;
    }
}

