namespace Market.Tests.Common.Base;

/// <summary>
/// Base class for validator tests with common assertions and utilities.
/// </summary>
public abstract class ValidatorTestBase : TestBase
{
    /// <summary>
    /// Asserts that a validation error exists for the specified field.
    /// </summary>
    protected void AssertHasError(IEnumerable<ValidationError> errors, string fieldName)
    {
        Assert.Contains(errors, e => e.Field == fieldName);
    }

    /// <summary>
    /// Asserts that no validation error exists for the specified field.
    /// </summary>
    protected void AssertHasNoError(IEnumerable<ValidationError> errors, string fieldName)
    {
        Assert.DoesNotContain(errors, e => e.Field == fieldName);
    }

    /// <summary>
    /// Asserts that the validation result is valid and has no errors.
    /// </summary>
    protected void AssertIsValid(ValidationResult result)
    {
        Assert.True(result.IsValid, $"Expected valid result but got errors: {string.Join(", ", result.Errors.Select(e => e.Field))}");
    }

    /// <summary>
    /// Asserts that the validation result is invalid.
    /// </summary>
    protected void AssertIsInvalid(ValidationResult result)
    {
        Assert.False(result.IsValid, "Expected invalid result but validation passed");
    }
}
