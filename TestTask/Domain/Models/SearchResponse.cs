namespace TestTask.Domain.Models;

public class SearchResponse
{
    // Mandatory
    // Array of routes
    public required Route[] Routes { get; set; }

    // Mandatory
    // The cheapest route
    public required decimal MinPrice { get; set; }

    // Mandatory
    // Most expensive route
    public required decimal MaxPrice { get; set; }

    // Mandatory
    // The fastest route
    public required int MinMinutesRoute { get; set; }

    // Mandatory
    // The longest route
    public required int MaxMinutesRoute { get; set; }
}