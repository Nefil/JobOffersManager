using System.Net;
using System.Text.Json;

namespace JobOffersManager.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            await HandleException(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception)
        {
            await HandleException(
                context,
                HttpStatusCode.InternalServerError,
                "Unexpected server error"
            );
        }
    }

    private static async Task HandleException(
        HttpContext context,
        HttpStatusCode statusCode,
        string message)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response)
        );
    }
}
