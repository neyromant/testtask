using TestTask.Domain.Interfaces;

namespace TestTask.Infrastructure.Services;

internal sealed class HealthCheckService : IHealthCheckService
{
    private readonly IEnumerable<IProvider> _providers;

    public HealthCheckService(IEnumerable<IProvider> providers)
    {
        _providers = providers ?? throw new ArgumentNullException(nameof(providers));
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
    {
        var pingTasks = _providers.Select(x => x.IsAvailableAsync(cancellationToken)).ToList();
        await Task.WhenAll(pingTasks);
        return pingTasks.Any(x => x.Result);
    }
}