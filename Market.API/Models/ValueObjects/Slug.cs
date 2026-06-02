using System.Text.RegularExpressions;

namespace Market.API.Models.ValueObjects
{
    /// <summary>
    /// Value object for URL-friendly slugs
    /// </summary>
    public class Slug
    {
        public string Value { get; private set; }

        public Slug(string value)
        {
            Value = value;
        }

        public static Slug Create(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be empty", nameof(text));

            var slug = text
                .ToLower()
                .Trim()
                .Replace(" ", "-")
                .Replace("_", "-")
                .Replace(".", "-");

            // Remove invalid characters
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
            // Remove multiple consecutive hyphens
            slug = Regex.Replace(slug, @"-+", "-");
            // Remove leading/trailing hyphens
            slug = slug.Trim('-');

            if (string.IsNullOrWhiteSpace(slug))
                throw new ArgumentException("Text cannot be converted to a valid slug", nameof(text));

            return new Slug(slug);
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj)
        {
            return obj is Slug slug && slug.Value == Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(Slug? a, Slug? b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Value == b.Value;
        }

        public static bool operator !=(Slug? a, Slug? b) => !(a == b);
    }
}
