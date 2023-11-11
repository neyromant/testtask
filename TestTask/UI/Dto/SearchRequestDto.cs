namespace TestTask.UI.Dto;

public class SearchRequestDto
{
    // Mandatory
    // Start point of route, e.g. Moscow 
    public required string Origin { get; set; }

    // Mandatory
    // End point of route, e.g. Sochi
    public required string Destination { get; set; }

    // Mandatory
    // Start date of route
    public required DateTime OriginDateTime { get; set; }

    // Optional
    public SearchFiltersDto? Filters { get; set; }
}