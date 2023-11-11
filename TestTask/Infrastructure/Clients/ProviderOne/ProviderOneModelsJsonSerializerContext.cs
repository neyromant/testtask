using System.Text.Json.Serialization;

namespace TestTask.Infrastructure.Clients.ProviderOne;

[JsonSourceGenerationOptions(
    WriteIndented = false,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(ProviderOneSearchResponse))]
[JsonSerializable(typeof(ProviderOneSearchRequest))]
internal partial class ProviderOneModelsJsonSerializerContext : JsonSerializerContext { }