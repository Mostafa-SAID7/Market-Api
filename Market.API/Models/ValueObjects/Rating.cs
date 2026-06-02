namespace Market.API.Models.ValueObjects
{
    /// <summary>
    /// Value object for ratings (1-5 stars)
    /// </summary>
    public class Rating
    {
        public int Value { get; private set; }

        private Rating(int value)
        {
            Value = value;
        }

        public static Rating Create(int value)
        {
            if (value < 1 || value > 5)
                throw new ArgumentException("Rating must be between 1 and 5", nameof(value));

            return new Rating(value);
        }

        public bool IsExcellent => Value >= 5;
        public bool IsGood => Value >= 4;
        public bool IsAverage => Value >= 3;
        public bool IsBad => Value <= 2;

        public override string ToString() => $"{Value}/5";

        public override bool Equals(object? obj)
        {
            return obj is Rating rating && rating.Value == Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(Rating? a, Rating? b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Value == b.Value;
        }

        public static bool operator !=(Rating? a, Rating? b) => !(a == b);
    }
}
