using TestTask.Domain.Exceptions;
using TestTask.Domain.Interfaces;
using TestTask.Domain.Models;

namespace TestTask.Infrastructure.Services;

internal sealed class SearchService : ISearchService
{
    private readonly IReadOnlyList<IProvider> _providers;
    private readonly IRoutesCache _cache;
    private readonly IRoutesFilter _filter;
    private readonly ISearchResultsAggregator _aggregator;
    private readonly ILogger _logger;

    public SearchService(
        IEnumerable<IProvider> providers, 
        IRoutesCache cache,
        IRoutesFilter filter,
        ISearchResultsAggregator aggregator,
        ILogger<SearchService> logger)
    {
        _providers = (providers ?? throw new ArgumentNullException(nameof(providers))).ToList();
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _filter = filter ?? throw new ArgumentNullException(nameof(filter));
        _aggregator = aggregator ?? throw new ArgumentNullException(nameof(aggregator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        if (request.Filters?.OnlyCached != true)
        {
            var searchResultState = await SearchViaProvidersAndFillCacheAsync(request, cancellationToken);
            if (searchResultState is SearchResultState.AllProvidersUnavailable or SearchResultState.NoRegisteredProviders)
            {
                _logger.LogError(searchResultState == SearchResultState.NoRegisteredProviders ? "No registered providers" : "All providers are unavailable");
                throw new SearchServiceUnavailableException();
            }
        }

        var allRoutes = _cache.GetAll();
        var routes = _filter.Apply(allRoutes, request);
        var result = _aggregator.Aggregate(routes);
        return result;
    }

    private async Task<SearchResultState> SearchViaProvidersAndFillCacheAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        if (_providers.Count == 0)
        {
            return SearchResultState.NoRegisteredProviders;
        }

        var searchOptions = new SearchOptions
        {
            DateFrom = request.OriginDateTime,
            DateTo = request.Filters?.DestinationDateTime,
            From = request.Origin,
            To = request.Destination,
            MaxPrice = request.Filters?.MaxPrice,
            MinTimeLimit = request.Filters?.MinTimeLimit,
        };

        var searchTasks = _providers.Select(provider => GetDataFromProviderAsync(provider, searchOptions, cancellationToken)).ToList();

        await Task.WhenAll(searchTasks);

        var searchResults = searchTasks.Select(t => t.Result).ToList();
        return searchResults.All(x => x)
            ? SearchResultState.Success
            : searchResults.All(x => x == false)
                ? SearchResultState.AllProvidersUnavailable
                : SearchResultState.SomeProvidersUnavailable;
    }

    private async Task<bool> GetDataFromProviderAsync(IProvider provider, SearchOptions searchOptions, CancellationToken cancellationToken)
    {
        try
        {
            var result = await provider.SearchAsync(searchOptions, cancellationToken);
            _cache.AddRange(result);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Exception occured on search via {providerName}", provider.Name);
            return false;
        }
    }
}