namespace Market.Application.Validators
{
    /// <summary>
    /// Base validator interface for all entities
    /// </summary>
    public interface IValidator<T>
    {
        /// <summary>
        /// Validate entity
        /// </summary>
        ValidationResult Validate(T entity);
    }

    /// <summary>
    /// Validation result containing errors
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; } = true;
        public List<ValidationError> Errors { get; set; } = new();

        public ValidationResult()
        {
        }

        public ValidationResult(List<ValidationError> errors)
        {
            Errors = errors;
            IsValid = errors.Count == 0;
        }

        public void AddError(string field, string message)
        {
            Errors.Add(new ValidationError { Field = field, Message = message });
            IsValid = false;
        }
    }

    /// <summary>
    /// Individual validation error
    /// </summary>
    public class ValidationError
    {
        public string Field { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}


