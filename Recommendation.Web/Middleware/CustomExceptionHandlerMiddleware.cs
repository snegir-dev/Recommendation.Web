using System.ComponentModel.DataAnnotations;
using System.Net;
using Newtonsoft.Json;
using Recommendation.Application.Common.Exceptions;

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

    private Task HandlerExceptionAsync(HttpContext context, Exception ex)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (ex)
        {
            case RecordExistsException recordExistsException:
                statusCode = HttpStatusCode.Conflict;
                result = JsonConvert.SerializeObject(recordExistsException.Message);
                break;
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                result = JsonConvert.SerializeObject(notFoundException.Message);
                break;
            case AuthenticationException authenticationException:
                statusCode = HttpStatusCode.Unauthorized;
                result = JsonConvert.SerializeObject(authenticationException.Message);
                break;
            case AccessDeniedException accessDeniedException:
                statusCode = HttpStatusCode.Forbidden;
                result = JsonConvert.SerializeObject(accessDeniedException.Message);
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        if (result == string.Empty)
            result = JsonConvert.SerializeObject(new { error = ex.Message });

        return context.Response.WriteAsync(result);
    }
}