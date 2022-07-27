using MyPetProject.Modularity;

namespace MyPetProject.WebApi;

public class ExceptionMiddleware
{
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            await HandleException(httpContext, exception);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        string message;
        switch (exception)
        {
            case AppException appException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                message = appException.Message;
                break;

            default:
                _logger.LogError(exception.ToString());
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                message = "Unknown internal error.";
                break;
        }

        await context.Response.WriteAsync(message);
    }

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
}

public static class ExceptionMiddlewareExtension
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}