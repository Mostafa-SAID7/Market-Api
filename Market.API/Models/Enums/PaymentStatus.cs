namespace Market.API.Models.Enums
{
    /// <summary>
    /// Payment statuses in the e-commerce platform
    /// </summary>
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Cancelled,
        Refunded
    }
}
