var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var routes = new List<ProviderOneRoute>
{
    new ProviderOneRoute
    {
        DateFrom = DateTime.Now,
        DateTo = DateTime.Now.AddHours(2),
        Price = 100500,
        TimeLimit = DateTime.Now.AddDays(1),
        From = "Moscow",
        To = "Sochi"
    },
    new ProviderOneRoute
    {
        DateFrom = DateTime.Now.AddDays(1),
        DateTo = DateTime.Now.AddDays(1).AddHours(6),
        Price = 100501,
        TimeLimit = DateTime.Now.AddDays(1),
        From = "Sochi",
        To = "Moscow"
    },
};

app.MapGet("/api/v1/ping", () => true);
app.MapPost("/api/v1/search", (ProviderOneSearchRequest request) =>
{
    return new ProviderOneSearchResponse
    {
        Routes = routes.ToArray()
    };

//return new ProviderOneSearchResponse
//    {
//        Routes = routes.Where(x => x.From.Equals(request.From, StringComparison.InvariantCultureIgnoreCase) &&
//                                   x.To.Equals(request.To, StringComparison.InvariantCultureIgnoreCase) && 
//                                   x.DateFrom >= request.DateFrom &&
//                                   x.DateTo <= request.DateTo &&
//                                   (!request.MaxPrice.HasValue || x.Price <= request.MaxPrice.Value)).ToArray()
//    };
});

app.Run();



public class ProviderOneSearchRequest
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
    // End date of route
    public DateTime? DateTo { get; set; }

    // Optional
    // Maximum price of route
    public decimal? MaxPrice { get; set; }
}

public class ProviderOneSearchResponse
{
    // Mandatory
    // Array of routes
    public ProviderOneRoute[] Routes { get; set; }
}

public class ProviderOneRoute
{
    // Mandatory
    // Start point of route
    public string From { get; set; }

    // Mandatory
    // End point of route
    public string To { get; set; }

    // Mandatory
    // Start date of route
    public DateTime DateFrom { get; set; }

    // Mandatory
    // End date of route
    public DateTime DateTo { get; set; }

    // Mandatory
    // Price of route
    public decimal Price { get; set; }

    // Mandatory
    // Timelimit. After it expires, route became not actual
    public DateTime TimeLimit { get; set; }
}