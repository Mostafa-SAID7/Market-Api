namespace Market.Tests.Common.Base;

/// <summary>
/// Base class for repository and data access tests.
/// Provides common setup for database context and mock repositories.
/// </summary>
public abstract class RepositoryTestBase : TestBase
{
    /// <summary>
    /// Creates a strict mock IUnitOfWork for testing command handlers.
    /// </summary>
    protected Mock<IUnitOfWork> CreateMockUnitOfWork()
        => CreateStrictMock<IUnitOfWork>();

    /// <summary>
    /// Creates a strict mock for a specific repository type.
    /// </summary>
    protected Mock<T> CreateMockRepository<T>() where T : class
        => CreateStrictMock<T>();

    /// <summary>
    /// Verifies that SaveAsync was called exactly once on the unit of work.
    /// </summary>
    protected void VerifySaveWasCalled(Mock<IUnitOfWork> unitOfWork)
    {
        unitOfWork.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Verifies that SaveAsync was never called on the unit of work.
    /// </summary>
    protected void VerifySaveWasNotCalled(Mock<IUnitOfWork> unitOfWork)
    {
        unitOfWork.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    /// <summary>
    /// Verifies that no other calls were made to the unit of work beyond what was explicitly setup.
    /// </summary>
    protected void VerifyNoOtherCalls(Mock<IUnitOfWork> unitOfWork)
    {
        unitOfWork.VerifyNoOtherCalls();
    }
}
