namespace ModularWebService.Modularity;

public class AppException : Exception
{
    public AppException(string message)
        : base(message)
    {
    }
}