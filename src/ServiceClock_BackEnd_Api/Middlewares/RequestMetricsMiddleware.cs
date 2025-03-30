using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Prometheus;

namespace ServiceClock_BackEnd_Api.Middlewares;

public class RequestMetricsMiddleware
{
    private readonly RequestDelegate _next;

    public RequestMetricsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            CustomMetrics.HttpRequestsTotal
                .Labels(context.Request.Method, context.Response.StatusCode.ToString())
                .Inc();
        }
    }
}

public static class CustomMetrics
{
    public static readonly Counter HttpRequestsTotal = Metrics.CreateCounter(
        "serviceclock_api_requests",
        "Total de requisições HTTP",
        new CounterConfiguration
        {
            LabelNames = new[] { "method", "status_code" }
        });
}
