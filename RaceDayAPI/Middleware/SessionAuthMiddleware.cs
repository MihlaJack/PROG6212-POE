using System.Net;
using System.Text.Json;

namespace RaceDayAPI.Middleware;

public class SessionAuthMiddleware
{
    private readonly RequestDelegate _next;

    public SessionAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Don't require login for register and login endpoints - that would be silly
        if (context.Request.Path.StartsWithSegments("/api/auth/login") || 
            context.Request.Path.StartsWithSegments("/api/auth/register"))
        {
            await _next(context);
            return;
        }

        // Check if they have an active session
        var userId = context.Session.GetInt32("UserId");
        var userRole = context.Session.GetString("UserRole");

        // No session? They need to log in first
        if (userId == null || userRole == null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";
            var response = new { message = "Unauthorized. Please login first." };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            return;
        }

        // Store the user info in the context so controllers can access it
        context.Items["UserId"] = userId;
        context.Items["UserRole"] = userRole;

        // All good, let the request continue
        await _next(context);
    }
}
