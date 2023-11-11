namespace TestTask.Domain.Models;

public class SearchOptions
{
    // Mandatory
    // Start point of route, e.g. Moscow 
    public string From { get; set; }

    // Mandatory
    // End point of route, e.g. Sochi
    public string To { get; set; }

    // Mandatory
    // Start date of route
    public DateTime DateFrom { get; set; }

    // Optional
    // Minimum value of timelimit for route
    public DateTime? MinTimeLimit { get; set; }
    
    // Optional
    // End date of route
    public DateTime? DateTo { get; set; }

    // Optional
    // Maximum price of route
    public decimal? MaxPrice { get; set; }
}