namespace Market.Domain.Entities
{
    /// <summary>
    /// ProductTag - junction entity for Product tags (relational model)
    /// </summary>
    public class ProductTag
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string TagName { get; set; } = string.Empty;

        // Navigation
        public Product Product { get; set; } = null!;
    }
}

