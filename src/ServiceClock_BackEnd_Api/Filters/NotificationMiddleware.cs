
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ServiceClock_BackEnd.Filters;

public class NotificationMiddleware : IAsyncResultFilter
{
    private readonly INotificationService notifications;
    public NotificationMiddleware(INotificationService notifications)
    {
        this.notifications = notifications;
    }
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (notifications.HasNotifications)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.HttpContext.Response.ContentType = "application/json";
            var obj = JsonConvert.SerializeObject(notifications.Notifications);
            await context.HttpContext.Response.WriteAsync(obj);
            return;
        }
        await next();
    }
}


