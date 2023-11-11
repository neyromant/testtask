using System.Text.Json.Serialization.Metadata;
using TestTask.Domain.Exceptions;

namespace TestTask.Infrastructure.Clients;

internal abstract class HttpClientBasedProviderBase<TSearchRequest, TSearchResponse>
{
    private readonly HttpClient _httpClient;

    internal HttpClientBasedProviderBase(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var response = await _httpClient.GetAsync(GetPingEndpointUri(), cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex) when (ex is HttpRequestException or OperationCanceledException)
        {
            return false;
        }
    }

    protected abstract Uri GetSearchEndpointUri();
    protected abstract Uri GetPingEndpointUri();
    protected abstract JsonTypeInfo<TSearchResponse> GetReponseJsonTypeInfo();
    protected abstract JsonTypeInfo<TSearchRequest> GetRequestJsonTypeInfo();

    protected async Task<TSearchResponse> SearchInternalAsync(TSearchRequest request, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.PostAsJsonAsync(GetSearchEndpointUri(), request, GetRequestJsonTypeInfo(), cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new ProviderUnavailableException(response.StatusCode);
        }

        var result = await response.Content.ReadFromJsonAsync(GetReponseJsonTypeInfo(), cancellationToken);

        if (result == null)
        {
            throw new ProviderInvalidResultException();
        }

        return result;
    }
}