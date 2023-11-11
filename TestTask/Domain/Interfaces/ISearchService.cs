using TestTask.Domain.Models;

namespace TestTask.Domain.Interfaces;

public interface ISearchService
{
    Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken);
}
