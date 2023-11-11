using TestTask.Domain.Models;
using Route = TestTask.Domain.Models.Route;

namespace TestTask.Domain.Interfaces;

public interface ISearchResultsAggregator
{
    SearchResponse Aggregate(IEnumerable<Route> items);
}