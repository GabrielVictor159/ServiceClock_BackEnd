
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs;
using ServiceClock_BackEnd.Api.Filters;
using ServiceClock_BackEnd.Api.UseCases.Company.Login;
using ServiceClock_BackEnd.Api.Validator.Http;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using System.Net;
using ServiceClock_BackEnd.Api.Helpers.Hateoas;

namespace ServiceClock_BackEnd.Api.UseCases.Client.Login;

public class LoginClient : UseCaseCore
{
    private readonly IRepository<Domain.Models.Client> repository;
    private readonly ITokenService tokenService;
    public LoginClient
        (HttpRequestValidator httpRequestValidator,
        NotificationMiddleware middleware,
        IRepository<Domain.Models.Client> repository,
        ITokenService tokenService)
        : base(httpRequestValidator, middleware)
    {
        this.repository = repository;
        this.tokenService = tokenService;
    }

    [FunctionName("LoginClient")]
    [OpenApiOperation(operationId: "LoginClient", tags: new[] { "Client" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(LoginClientRequest), Description = "Request body containing company information.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(object), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Client", "related", "/LoginClient", "POST", typeof(LoginClientRequest))]
    public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {

        return await Execute(req, async (LoginClientRequest request) =>
        {
            if (request != null)
            {
                var company = this.repository.Find(e =>
                e.Email.Equals(request.Email)
                && e.Password.Equals(request.Password))
                .FirstOrDefault();

                if (company == null)
                {
                    return new BadRequestObjectResult("Login Invalid");
                }
                var token = this.tokenService.Generate("Company", company.Id);
                return new OkObjectResult(new { Token = token, _links = HateoasScheme.Instance.GetLinks("Client") });
            }
            return new OkResult();
        });
    }
}

    