
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceClock_BackEnd.Api.Validator.Http;
using ServiceClock_BackEnd.Application.Interfaces.Services;

namespace ServiceClock_BackEnd.Api.Filters;

public class NotificationMiddleware
{
    private readonly INotificationService _notifications;

    public NotificationMiddleware(INotificationService notifications)
    {
        _notifications = notifications;
    }

    public async Task<IActionResult> InvokeAsync(HttpRequest req, HttpRequestValidator httpRequestValidator, Func<Task<IActionResult>> next)
    {
        var validateHttp = await httpRequestValidator.Validate(req);
        if (!validateHttp.Item1)
        {
            return validateHttp.Item2!;
        }

        var result = await next();

        if (_notifications.HasNotifications)
        {
            var obj = JsonConvert.SerializeObject(_notifications.Notifications);
            var badRequest = new BadRequestObjectResult(obj);
            _notifications.Notifications.Clear();
            return badRequest;
        }

        return result;
    }
}

