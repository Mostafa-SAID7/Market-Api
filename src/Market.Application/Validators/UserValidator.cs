using Market.Domain.Entities;

namespace Market.Application.Validators
{
    /// <summary>
    /// Validator for User entity
    /// </summary>
    public class UserValidator : IValidator<User>
    {
        private static readonly System.Text.RegularExpressions.Regex EmailRegex = 
            new(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");

        public ValidationResult Validate(User entity)
        {
            var result = new ValidationResult();

            // Email validation
            if (string.IsNullOrWhiteSpace(entity.Email))
                result.AddError(nameof(User.Email), "Email is required");
            else if (!EmailRegex.IsMatch(entity.Email))
                result.AddError(nameof(User.Email), "Email format is invalid");
            else if (entity.Email.Length > 255)
                result.AddError(nameof(User.Email), "Email cannot exceed 255 characters");

            // PasswordHash validation
            if (string.IsNullOrWhiteSpace(entity.PasswordHash))
                result.AddError(nameof(User.PasswordHash), "Password hash is required");
            else if (entity.PasswordHash.Length < 20)
                result.AddError(nameof(User.PasswordHash), "Password hash is invalid");

            // FirstName validation
            if (string.IsNullOrWhiteSpace(entity.FirstName))
                result.AddError(nameof(User.FirstName), "First name is required");
            else if (entity.FirstName.Length > 100)
                result.AddError(nameof(User.FirstName), "First name cannot exceed 100 characters");

            // LastName validation
            if (string.IsNullOrWhiteSpace(entity.LastName))
                result.AddError(nameof(User.LastName), "Last name is required");
            else if (entity.LastName.Length > 100)
                result.AddError(nameof(User.LastName), "Last name cannot exceed 100 characters");

            // PhoneNumber validation
            if (!string.IsNullOrWhiteSpace(entity.PhoneNumber))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(entity.PhoneNumber, @"^\+?[1-9]\d{1,14}$"))
                    result.AddError(nameof(User.PhoneNumber), "Phone number format is invalid");
            }

            return result;
        }
    }
}


