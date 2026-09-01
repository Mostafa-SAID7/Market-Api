using Market.Domain.Common;

namespace Market.Domain.Entities
{
    /// <summary>
    /// Shopping cart entity
    /// </summary>
    public class Cart : BaseEntity
    {
        public int UserId { get; set; }
        
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
        
        public decimal SubTotal => Items.Sum(x => x.SubTotal);
        public int TotalItems => Items.Sum(x => x.Quantity);

        // Navigation properties
        public User User { get; set; } = null!;

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
        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                Items.Remove(item);
                UpdatedAt = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Update item quantity
        /// </summary>
        public void UpdateItemQuantity(int productId, int quantity)
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
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int VendorId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        public decimal SubTotal => Price * Quantity;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public Cart Cart { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public Vendor Vendor { get; set; } = null!;
    }
}


