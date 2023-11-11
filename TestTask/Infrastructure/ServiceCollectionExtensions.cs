using TestTask.Domain.Interfaces;
using TestTask.Infrastructure.Clients;
using TestTask.Infrastructure.Services;

namespace TestTask.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddProvidersClients(configuration);

        services.AddScoped<ISearchService, SearchService>();
        services.AddScoped<IHealthCheckService, HealthCheckService>();
        services.AddSingleton<IRoutesCache, RoutesCache>();
        services.AddSingleton<IRoutesFilter, RoutesFilter>();
        services.AddSingleton<ISearchResultsAggregator, SearchResultsAggregator>();
        return services;
    } 
}