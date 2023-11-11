using TestTask.Domain.Models;
using Route = TestTask.Domain.Models.Route;

namespace TestTask.Domain.Interfaces;

public interface IProvider
{
    string Name { get; }
    
    Task<IReadOnlyList<Route>> SearchAsync(SearchOptions searchOptions, CancellationToken cancellationToken);

    Task<bool> IsAvailableAsync(CancellationToken cancellationToken);
}