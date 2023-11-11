using TestTask.Domain.Interfaces;
using TestTask.Domain.Models;
using Route = TestTask.Domain.Models.Route;

namespace TestTask.Infrastructure.Services;

internal sealed class RoutesFilter : IRoutesFilter
{
    public IEnumerable<Route> Apply(IEnumerable<Route> routes, SearchRequest request)
    {
        var resultQuery = routes.AsParallel()
            .Where(x => x.Origin.Equals(request.Origin, StringComparison.OrdinalIgnoreCase) && 
                        x.OriginDateTime >= request.OriginDateTime && 
                        x.Destination.Equals(request.Destination, StringComparison.OrdinalIgnoreCase));

        if (request.Filters != null)
        {
            if (request.Filters.DestinationDateTime.HasValue)
            {
                resultQuery = resultQuery.Where(x => x.DestinationDateTime <= request.Filters.DestinationDateTime);
            }

            if (request.Filters.MaxPrice.HasValue)
            {
                resultQuery = resultQuery.Where(x => x.Price <= request.Filters.MaxPrice.Value);
            }

            if (request.Filters.MinTimeLimit.HasValue)
            {
                resultQuery = resultQuery.Where(x => x.TimeLimit >= request.Filters.MinTimeLimit.Value);
            }
        }

        return resultQuery.ToList();
    }
}