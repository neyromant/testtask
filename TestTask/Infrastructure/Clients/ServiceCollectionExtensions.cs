using Microsoft.Extensions.Options;
using TestTask.Domain.Interfaces;
using TestTask.Infrastructure.Clients.ProviderOne;
using TestTask.Infrastructure.Clients.ProviderTwo;

namespace TestTask.Infrastructure.Clients;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProvidersClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ProviderOneClientOptions>(configuration.GetSection(nameof(ProviderOneClientOptions)));
        services.AddHttpClient<ProviderOneClient>((sp, client) =>
        {
            ConfigureHttpClient(client, sp.GetRequiredService<IOptions<ProviderOneClientOptions>>().Value);
        });
        services.AddScoped<IProvider>(sp => sp.GetRequiredService<ProviderOneClient>());

        services.Configure<ProviderTwoClientOptions>(configuration.GetSection(nameof(ProviderTwoClientOptions)));
        services.AddHttpClient<ProviderTwoClient>((sp, client) =>
        {
            ConfigureHttpClient(client, sp.GetRequiredService<IOptions<ProviderTwoClientOptions>>().Value);
        });
        services.AddScoped<IProvider>(sp => sp.GetRequiredService<ProviderTwoClient>());

        return services;
    }

    private static void ConfigureHttpClient(HttpClient client, ProviderClientOptions options)
    {
        client.BaseAddress = new Uri(options.BaseAddress, UriKind.Absolute);
        client.Timeout = options.TimeOut;
        // TODO: Имеет смыл накидать политик retry/circuit breaker/bulkhead, например, с помощью Polly, а параметры забирать из конфигурации
    }
}