namespace Market.Application.Features.Carts
{
    /// <summary>
    /// Cart response DTO
    /// </summary>
    public class CartResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<CartItemResponse> Items { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// Cart item response DTO
    /// </summary>
    public class CartItemResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int VendorId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public decimal SubTotal { get; set; }
    }
}


