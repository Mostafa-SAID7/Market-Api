namespace Market.API.Models.ValueObjects
{
    /// <summary>
    /// Value object for product tags
    /// </summary>
    public class Tag
    {
        public string Name { get; private set; }

        private Tag(string name)
        {
            Name = name;
        }

        public static Tag Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tag name cannot be empty", nameof(name));

            var trimmedName = name.Trim();

            if (trimmedName.Length > 50)
                throw new ArgumentException("Tag name cannot exceed 50 characters", nameof(name));

            return new Tag(trimmedName);
        }

        public override string ToString() => Name;

        public override bool Equals(object? obj)
        {
            return obj is Tag tag && tag.Name.Equals(Name, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Name.ToLower().GetHashCode();
        }

        public static bool operator ==(Tag? a, Tag? b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(Tag? a, Tag? b) => !(a == b);
    }
}
