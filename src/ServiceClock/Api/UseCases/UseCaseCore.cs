﻿
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceClock_BackEnd.Api.Filters;
using ServiceClock_BackEnd.Api.Validator.Http;
using System.IO;
using AutoMapper;
using System;

namespace ServiceClock_BackEnd.Api.UseCases;

public abstract class UseCaseCore
{
    protected readonly HttpRequestValidator httpRequestValidator;
    protected readonly NotificationMiddleware middleware;

    protected UseCaseCore(HttpRequestValidator httpRequestValidator, NotificationMiddleware middleware)
    {
        this.httpRequestValidator = httpRequestValidator;
        this.middleware = middleware;
    }

    public async Task<IActionResult> Execute<RequestType>(HttpRequest req,
             Func<RequestType, Task<IActionResult>> next)
    {
        this.httpRequestValidator.AddValidator(new BodyValidator<RequestType>());
        return await middleware.InvokeAsync(req, httpRequestValidator, async () =>
        {
            try 
            {
                req.Body.Position = 0;
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                RequestType body = JsonConvert.DeserializeObject<RequestType>(requestBody!)!;
                var result = await next(body);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Occurring an error: {ex.Message ?? ex.InnerException?.Message}, stacktrace: {ex.StackTrace}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        });
    }
}


