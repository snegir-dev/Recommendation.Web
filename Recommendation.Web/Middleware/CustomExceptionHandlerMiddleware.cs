using System.Net;
using Newtonsoft.Json;
using Recommendation.Application.Common.Exceptions;
using ValidationException = FluentValidation.ValidationException;

namespace Recommendation.Web.Middleware;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

    public CustomExceptionHandlerMiddleware(RequestDelegate next,
        ILogger<CustomExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error - {E}", e);
            await HandlerExceptionAsync(context, e);
        }
    }

    private static Task HandlerExceptionAsync(HttpContext context, Exception ex)
    {
        var statusCode = HttpStatusCode.InternalServerError;

        statusCode = ex switch
        {
            RecordExistsException => HttpStatusCode.Conflict,
            NotFoundException => HttpStatusCode.NotFound,
            AuthenticationException => HttpStatusCode.Unauthorized,
            AccessDeniedException => HttpStatusCode.Forbidden,
            InternalServerException => HttpStatusCode.InternalServerError,
            ValidationException => HttpStatusCode.BadRequest,
            _ => statusCode
        };

        var result = JsonConvert.SerializeObject(ex.Message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        if (result == string.Empty)
            result = JsonConvert.SerializeObject(new { error = ex.Message });

        return context.Response.WriteAsync(result);
    }
}