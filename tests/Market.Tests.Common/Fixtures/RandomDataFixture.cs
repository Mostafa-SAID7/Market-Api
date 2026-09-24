namespace Market.Tests.Common.Fixtures;

/// <summary>
/// Provides random test data generation for common scenarios.
/// Used to create varied test cases without explicit data builders.
/// </summary>
public class RandomDataFixture
{
    private static readonly Random _random = new();

    /// <summary>
    /// Generates a random positive decimal value within reasonable bounds.
    /// </summary>
    public decimal RandomPrice(decimal min = 0.01m, decimal max = 10000m)
        => Math.Round((decimal)_random.NextDouble() * (max - min) + min, 2);

    /// <summary>
    /// Generates a random integer quantity between 1 and 1000.
    /// </summary>
    public int RandomQuantity(int min = 1, int max = 1000)
        => _random.Next(min, max + 1);

    /// <summary>
    /// Generates a random rating between 1 and 5.
    /// </summary>
    public int RandomRating()
        => _random.Next(1, 6);

    /// <summary>
    /// Generates a random commission rate between 0% and 50%.
    /// </summary>
    public decimal RandomCommissionRate()
        => Math.Round((decimal)_random.NextDouble() * 0.50m, 4);

    /// <summary>
    /// Generates a random positive ID (typically 1-999999).
    /// </summary>
    public int RandomId(int min = 1, int max = 999999)
        => _random.Next(min, max + 1);

    /// <summary>
    /// Generates a random email address for testing.
    /// </summary>
    public string RandomEmail()
        => $"user{_random.Next(100000)}@example.com";

    /// <summary>
    /// Generates a random string of specified length.
    /// </summary>
    public string RandomString(int length = 10)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Range(0, length)
            .Select(_ => chars[_random.Next(chars.Length)])
            .ToArray());
    }
}
