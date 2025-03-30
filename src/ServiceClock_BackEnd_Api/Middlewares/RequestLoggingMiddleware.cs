using System;
using System.Text;

namespace ServiceClock_BackEnd_Api.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("Iniciando requisição: {method} {url}", context.Request.Method, context.Request.Path);
        

        foreach (var header in context.Request.Headers)
        {
            _logger.LogInformation("Header: {key} = {value}", header.Key, header.Value);
        }

        context.Request.EnableBuffering(); 
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
        {
            var body = await reader.ReadToEndAsync();
            _logger.LogInformation("Corpo da requisição: {body}", body);
            context.Request.Body.Position = 0; 
        }

        await _next(context);
    }
}
