using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using TestTask.Domain.Interfaces;
using Route = TestTask.Domain.Models.Route;

namespace TestTask.Infrastructure.Services;

internal sealed class RoutesCache : IRoutesCache
{
    private readonly IMemoryCache _cache;
    private readonly ConcurrentDictionary<string, Route> _allItems = new();

    public RoutesCache(IMemoryCache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public Route? TryGetById(Guid id)
    {
        return _cache.TryGetValue<Route>(id, out var result)
            ? result
            : null;
    }

    public IEnumerable<Route> GetAll()
    {
        return _allItems.Values;
    }

    public void AddRange(IEnumerable<Route> items)
    {
        foreach (var item in items)
        {
            var itemKey = GetRouteKey(item);
            if (!_allItems.TryAdd(itemKey, item))
            {
                continue;
            }

            using var entry = _cache.CreateEntry(item.Id);
            entry.SetValue(item)
                .SetAbsoluteExpiration(item.TimeLimit)
                .RegisterPostEvictionCallback(
                    (key, value, reason, state) =>
                    {
                        _allItems.TryRemove(GetRouteKey(item), out _);
                    });
        }
    }

    private static string GetRouteKey(Route route)
    {
        return $"{route.Origin}_{route.Destination}_{route.OriginDateTime:O}_{route.DestinationDateTime:O}_{route.Price:F}_${route.TimeLimit:O}";
    }
}