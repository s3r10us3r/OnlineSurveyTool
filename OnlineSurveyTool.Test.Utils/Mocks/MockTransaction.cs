using Microsoft.EntityFrameworkCore.Storage;

namespace OnlineSurveyTool.Test.Utils.Mocks;

public class MockTransaction : IDbContextTransaction 
{
    public void Dispose()
    {
    }

    public async ValueTask DisposeAsync()
    {
    }

    public void Commit()
    {
    }

    public async Task CommitAsync(CancellationToken cancellationToken = new CancellationToken())
    {
    }

    public void Rollback()
    {
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = new CancellationToken())
    {
    }

    public Guid TransactionId { get; }
}