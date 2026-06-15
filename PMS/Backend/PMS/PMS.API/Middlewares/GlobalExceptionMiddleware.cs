using PMS.Application.Exceptions;

namespace PMS.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ArgumentException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var message = exception switch
        {
            NotFoundException => exception.Message,
            ArgumentException => exception.Message,
            _ => "Something went wrong on the server."
        };

        var response = new
        {
            statusCode = statusCode,
            message = message,
            path = context.Request.Path.Value,
            traceId = context.TraceIdentifier,
            timeStamp = DateTime.UtcNow
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(response);
    }
}