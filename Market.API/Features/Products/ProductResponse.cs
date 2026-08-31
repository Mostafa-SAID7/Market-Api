namespace Market.API.Features.Products
{
    /// <summary>
    /// Product response DTO
    /// </summary>
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
        public int Sold { get; set; }
        public int CategoryId { get; set; }
        public int VendorId { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}
