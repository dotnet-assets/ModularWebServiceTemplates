using System.Net;

namespace ModularWebService.Modularity;

public class WebServiceException : Exception
{
    public WebServiceException(string message)
        : base(message)
    {
    }
}