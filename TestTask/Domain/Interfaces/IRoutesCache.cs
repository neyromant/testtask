using Route = TestTask.Domain.Models.Route;

namespace TestTask.Domain.Interfaces;

public interface IRoutesCache
{
    Route? TryGetById(Guid id);
    IEnumerable<Route> GetAll();
    void AddRange(IEnumerable<Route> items);
}