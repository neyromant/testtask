using System.Text.Json.Serialization;

namespace TestTask.Infrastructure.Clients.ProviderTwo;

[JsonSourceGenerationOptions(
    WriteIndented = false,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(ProviderTwoSearchResponse))]
[JsonSerializable(typeof(ProviderTwoSearchRequest))]
internal partial class ProviderTwoModelsJsonSerializerContext : JsonSerializerContext { }