namespace TestTask.Domain.Models;

public class Route
{
    // Mandatory
    // Identifier of the whole route
    public required Guid Id { get; set; }

    // Mandatory
    // Start point of route
    public required string Origin { get; set; }

    // Mandatory
    // End point of route
    public required string Destination { get; set; }

    // Mandatory
    // Start date of route
    public required DateTime OriginDateTime { get; set; }

    // Mandatory
    // End date of route
    public required DateTime DestinationDateTime { get; set; }

    // Mandatory
    // Price of route
    public required decimal Price { get; set; }

    // Mandatory
    // Timelimit. After it expires, route became not actual
    public required DateTime TimeLimit { get; set; }
}