using System.Text.Json.Serialization.Metadata;
using TestTask.Domain.Interfaces;
using TestTask.Domain.Models;
using Route = TestTask.Domain.Models.Route;

namespace TestTask.Infrastructure.Clients.ProviderOne;

// TODO: Стоит ProviderOneClient и ProviderTwoClient оставить именно клиентами, а IProvider реализовать в классах - адаптерах
internal sealed class ProviderOneClient : HttpClientBasedProviderBase<ProviderOneSearchRequest, ProviderOneSearchResponse>, IProvider
{
    private static readonly Uri SearchEndpointUri = new("api/v1/search", UriKind.RelativeOrAbsolute);
    private static readonly Uri PingEndpointUri = new("api/v1/ping", UriKind.RelativeOrAbsolute);

    public string Name => "ProviderOne";

    public ProviderOneClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<IReadOnlyList<Route>> SearchAsync(SearchOptions searchOptions, CancellationToken cancellationToken)
    {
        var request = new ProviderOneSearchRequest
        {
            DateFrom = searchOptions.DateFrom,
            DateTo = searchOptions.DateTo,
            From = searchOptions.From,
            MaxPrice = searchOptions.MaxPrice,
            To = searchOptions.To,
        };

        var response = await SearchInternalAsync(request, cancellationToken);

        return response.Routes.Select( x => new Route
        {
            Id = Guid.NewGuid(),
            Destination = x.To,
            DestinationDateTime = x.DateTo,
            Origin = x.From,
            OriginDateTime = x.DateFrom,
            Price = x.Price,
            TimeLimit = x.TimeLimit,
        }).ToList();
    }

    protected override Uri GetPingEndpointUri() => PingEndpointUri;
    protected override Uri GetSearchEndpointUri() => SearchEndpointUri;
    protected override JsonTypeInfo<ProviderOneSearchRequest> GetRequestJsonTypeInfo() => ProviderOneModelsJsonSerializerContext.Default.ProviderOneSearchRequest;
    protected override JsonTypeInfo<ProviderOneSearchResponse> GetReponseJsonTypeInfo() => ProviderOneModelsJsonSerializerContext.Default.ProviderOneSearchResponse;
}