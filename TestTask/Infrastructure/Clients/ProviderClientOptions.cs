namespace TestTask.Infrastructure.Clients;

public class ProviderClientOptions
{
    public required string BaseAddress { get; set; }
    public required TimeSpan TimeOut { get; set; } = TimeSpan.FromMinutes(1);
}