using System;

namespace ServiceClock_BackEnd_Api.Middlewares;

public class CustomMetricAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _requiredPassword = $"Bearer {Environment.GetEnvironmentVariable("METRICS_PASSWORD")!}";

    public CustomMetricAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/metrics"))
        {
            var authorizationHeader = context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Missing or invalid Authorization header.");
                return;
            }

            var token = authorizationHeader.Trim();

            if (!token.Equals(_requiredPassword))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden: Invalid password.");
                return;
            }
        }
        await _next(context); 
    }
}
