using System.Text.Json.Serialization.Metadata;
using TestTask.Domain.Interfaces;
using TestTask.Domain.Models;
using Route = TestTask.Domain.Models.Route;

namespace TestTask.Infrastructure.Clients.ProviderTwo;

internal sealed class ProviderTwoClient : HttpClientBasedProviderBase<ProviderTwoSearchRequest, ProviderTwoSearchResponse>, IProvider
{
    private static readonly Uri SearchEndpointUri = new("api/v1/search", UriKind.RelativeOrAbsolute);
    private static readonly Uri PingEndpointUri = new("api/v1/ping", UriKind.RelativeOrAbsolute);
    
    public string Name => "ProviderTwo";

    public ProviderTwoClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<IReadOnlyList<Route>> SearchAsync(SearchOptions searchOptions, CancellationToken cancellationToken)
    {
        var request = new ProviderTwoSearchRequest
        {
            Arrival = searchOptions.From,
            Departure = searchOptions.To,
            DepartureDate = searchOptions.DateFrom,
            MinTimeLimit = searchOptions.MinTimeLimit,
        };

        var response = await SearchInternalAsync(request, cancellationToken);

        return response.Routes.Select(x => new Route
        {
            Id = Guid.NewGuid(),
            Destination = x.Arrival.Point,
            DestinationDateTime = x.Arrival.Date,
            Origin = x.Departure.Point,
            OriginDateTime = x.Departure.Date,
            Price = x.Price,
            TimeLimit = x.TimeLimit,
        }).ToList();
    }

    protected override Uri GetSearchEndpointUri() => SearchEndpointUri;
    protected override Uri GetPingEndpointUri() => PingEndpointUri;

    protected override JsonTypeInfo<ProviderTwoSearchRequest> GetRequestJsonTypeInfo() => ProviderTwoModelsJsonSerializerContext.Default.ProviderTwoSearchRequest;
    protected override JsonTypeInfo<ProviderTwoSearchResponse> GetReponseJsonTypeInfo() => ProviderTwoModelsJsonSerializerContext.Default.ProviderTwoSearchResponse;
}