using System.Net;

namespace TestTask.Domain.Exceptions;

public class ProviderUnavailableException : Exception
{
    public ProviderUnavailableException(HttpStatusCode statusCode) : base($"Provider return {statusCode}")
    {

    }
}