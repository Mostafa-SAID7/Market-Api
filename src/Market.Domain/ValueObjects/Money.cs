namespace Market.Domain.ValueObjects
{
    /// <summary>
    /// Value object for monetary amounts
    /// </summary>
    public class Money
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = "USD";

        private Money(decimal amount, string currency = "USD")
        {
            Amount = amount;
            Currency = currency;
        }

        public static Money Create(decimal amount, string currency = "USD")
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative", nameof(amount));

            if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
                throw new ArgumentException("Currency must be a valid 3-letter code", nameof(currency));

            return new Money(amount, currency.ToUpper());
        }

        public Money Add(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException("Cannot add amounts in different currencies");

            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException("Cannot subtract amounts in different currencies");

            if (Amount < other.Amount)
                throw new InvalidOperationException("Result would be negative");

            return new Money(Amount - other.Amount, Currency);
        }

        public Money Multiply(decimal factor)
        {
            if (factor < 0)
                throw new ArgumentException("Factor cannot be negative", nameof(factor));

            return new Money(Amount * factor, Currency);
        }

        public override string ToString() => $"{Currency} {Amount:F2}";

        public override bool Equals(object? obj)
        {
            return obj is Money money && money.Amount == Amount && money.Currency == Currency;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }

        public static bool operator ==(Money? a, Money? b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(Money? a, Money? b) => !(a == b);

        public static bool operator <(Money? a, Money? b)
        {
            if (a is null || b is null) return false;
            if (a.Currency != b.Currency) throw new InvalidOperationException("Cannot compare different currencies");
            return a.Amount < b.Amount;
        }

        public static bool operator >(Money? a, Money? b)
        {
            if (a is null || b is null) return false;
            if (a.Currency != b.Currency) throw new InvalidOperationException("Cannot compare different currencies");
            return a.Amount > b.Amount;
        }
    }
}

