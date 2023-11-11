using TestTask.Domain.Interfaces;
using TestTask.Domain.Models;
using Route = TestTask.Domain.Models.Route;

namespace TestTask.Infrastructure.Services;

internal sealed class SearchResultsAggregator : ISearchResultsAggregator
{
    public SearchResponse Aggregate(IEnumerable<Route> items)
    {
        decimal? maxPrice = null;
        decimal? minPrice = null;
        int? minMinutesRoute = null;
        int? maxMinutesRoute = null;

        var routes = items.Select(x =>
        {
            if (!maxPrice.HasValue || x.Price > maxPrice)
            {
                maxPrice = x.Price;
            }

            if (!minPrice.HasValue || x.Price < minPrice)
            {
                minPrice = x.Price;
            }

            var totalMinutes = (int)(x.DestinationDateTime - x.OriginDateTime).TotalMinutes;
            if (!maxMinutesRoute.HasValue || totalMinutes > maxMinutesRoute)
            {
                maxMinutesRoute = totalMinutes;
            }

            if (!minMinutesRoute.HasValue || totalMinutes < minMinutesRoute)
            {
                minMinutesRoute = totalMinutes;
            }

            return x;
        }).ToArray();

        return new SearchResponse
        {
            Routes = routes,
            MaxPrice = maxPrice ?? 0,
            MinPrice = minPrice ?? 0,
            MaxMinutesRoute = maxMinutesRoute ?? 0,
            MinMinutesRoute = minMinutesRoute ?? 0,
        };
    }
}