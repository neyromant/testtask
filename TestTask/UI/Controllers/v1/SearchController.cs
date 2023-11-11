using System.Net;
using Microsoft.AspNetCore.Mvc;
using TestTask.Domain.Exceptions;
using TestTask.Domain.Interfaces;
using TestTask.Domain.Models;
using TestTask.UI.Dto;

namespace TestTask.UI.Controllers.v1
{
    [ApiController]
    [Route("v1/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        public SearchController(ISearchService searchService)
        {
            _searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
        }

        [HttpPost("search")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDto<SearchErrorCode>))]
        public async Task<ActionResult<SearchResponse>> SearchAsync([FromBody] SearchRequestDto request, CancellationToken cancellationToken)
        {
            var requestModel = new SearchRequest
            {
                Destination = request.Destination,
                Origin = request.Origin,
                OriginDateTime = request.OriginDateTime,
                Filters = request.Filters != null
                    ? new SearchFilters
                    {
                        DestinationDateTime = request.Filters.DestinationDateTime,
                        MaxPrice = request.Filters.MaxPrice,
                        MinTimeLimit = request.Filters.MinTimeLimit,
                        OnlyCached = request.Filters.OnlyCached,
                    }
                    : null
            };

            try
            {
                var response = await _searchService.SearchAsync(requestModel, cancellationToken);
                return response;
            }
            catch (SearchServiceUnavailableException)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorDto<SearchErrorCode>(SearchErrorCode.SearchEngineUnavailable, "Search engine unavailable"));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorDto<SearchErrorCode>(SearchErrorCode.InternalError, "Exception occured"));
            }
        }
    }
}