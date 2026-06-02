namespace Market.API.Models.Enums
{
    /// <summary>
    /// Order statuses in the e-commerce platform
    /// </summary>
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Processing,
        Shipped,
        Delivered,
        Cancelled,
        Refunded
    }
}
