﻿
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceClock_BackEnd.Api.Validator.Http;
using ServiceClock_BackEnd.Application.Interfaces.Services;

namespace ServiceClock_BackEnd.Api.Filters;

public class NotificationMiddleware
{
    private readonly INotificationService notifications;
    private readonly ILogService logService;

    public NotificationMiddleware(INotificationService notifications, ILogService logService)
    {
        this.notifications = notifications;
        this.logService = logService;
    }

    public async Task<IActionResult> InvokeAsync(HttpRequest req, HttpRequestValidator httpRequestValidator, Func<Task<IActionResult>> next)
    {
        var validateHttp = await httpRequestValidator.Validate(req);
        if (!validateHttp.Item1)
        {
            return validateHttp.Item2!;
        }

        var result = await next();
        
        this.logService.PopulateLogs();

        if (notifications.HasNotifications)
        {
            var obj = JsonConvert.SerializeObject(notifications.Notifications);
            var badRequest = new BadRequestObjectResult(obj);
            notifications.Notifications.Clear();
            return badRequest;
        }

        return result;
    }
}

