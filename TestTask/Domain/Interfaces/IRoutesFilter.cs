using TestTask.Domain.Models;
using Route = TestTask.Domain.Models.Route;

namespace TestTask.Domain.Interfaces;

public interface IRoutesFilter
{
    IEnumerable<Route> Apply(IEnumerable<Route> routes, SearchRequest request);
}