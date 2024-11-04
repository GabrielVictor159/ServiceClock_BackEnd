
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs;
using Microsoft.OpenApi.Models;
using ServiceClock_BackEnd.Api.Filters;
using ServiceClock_BackEnd.Api.Helpers.Hateoas;
using ServiceClock_BackEnd.Api.UseCases.Client.CreateClient;
using ServiceClock_BackEnd.Validator.Http
using ServiceClock_BackEnd.Application.UseCases.Client.CreateClient;
using System.Net;

namespace ServiceClock_BackEnd.UseCases.Auth.IsAuthenticated;

public class IsAuthenticated : UseCaseCore
{
    public IsAuthenticated
        (HttpRequestValidator httpRequestValidator, 
        NotificationMiddleware middleware) 
        : base(httpRequestValidator, middleware)
    {
    }

    [FunctionName("IsAuthenticated")]
    [OpenApiOperation(operationId: "IsAuthenticated", tags: new[] { "Auth" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(object), Description = "Request body containing company information.")]
    [OpenApiSecurity("bearer",
                     SecuritySchemeType.ApiKey,
                     In = OpenApiSecurityLocationType.Query,
                     Name = "code")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Auth", "auth", "/IsAuthenticated", "POST", typeof(object))]
    public async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        var authentication = new AuthorizationValidator();
        var (isAuthenticated, actionResult, claims) = await authentication.Validate(req);

        if (!isAuthenticated)
        {
            return actionResult ?? new UnauthorizedObjectResult("Authentication failed.");
        }

        return new OkObjectResult("User is authenticated successfully.");

    }
}

