using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace OnlineSurveyTool.Server.Services.Test.Mocks;

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