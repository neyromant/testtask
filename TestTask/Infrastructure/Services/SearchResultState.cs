namespace TestTask.Infrastructure.Services;

internal enum SearchResultState
{
    Success,
    SomeProvidersUnavailable,
    AllProvidersUnavailable,
    NoRegisteredProviders
}