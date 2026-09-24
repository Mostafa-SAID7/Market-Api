namespace Market.Tests.Common.Base;

/// <summary>
/// Base class for all unit tests providing common setup and utilities.
/// </summary>
public abstract class TestBase
{
    /// <summary>
    /// Creates a strict mock for a repository interface.
    /// Strict mocks throw when called without explicit setup.
    /// </summary>
    protected Mock<T> CreateStrictMock<T>() where T : class
        => new(MockBehavior.Strict);

    /// <summary>
    /// Creates a loose mock for a repository interface.
    /// Loose mocks return default values for unsetup calls.
    /// </summary>
    protected Mock<T> CreateLooseMock<T>() where T : class
        => new(MockBehavior.Loose);
}
