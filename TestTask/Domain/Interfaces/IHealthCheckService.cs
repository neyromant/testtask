namespace TestTask.Domain.Interfaces;

public interface IHealthCheckService
{
    Task<bool> IsAvailableAsync(CancellationToken cancellationToken);
}