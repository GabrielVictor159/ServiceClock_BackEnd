
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs;
using ServiceClock_BackEnd.Api.Filters;
using ServiceClock_BackEnd.Api.Validator.Http;
using System.Net;
using ServiceClock_BackEnd.Api.UseCases.Company.GetCompany;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Domain.Helpers;
using ServiceClock_BackEnd.Api.Helpers.Hateoas;

namespace ServiceClock_BackEnd.Api.UseCases.Company.Login;

public class LoginCompany : UseCaseCore
{
    private readonly IRepository<Domain.Models.Company> repository;
    private readonly ITokenService tokenService;
    public LoginCompany
        (HttpRequestValidator httpRequestValidator, 
        NotificationMiddleware middleware,
        IRepository<Domain.Models.Company> repository,
        ITokenService tokenService) 
        : base(httpRequestValidator, middleware)
    {
        this.repository = repository;
        this.tokenService = tokenService;
    }

    [FunctionName("LoginCompany")]
    [OpenApiOperation(operationId: "LoginCompany", tags: new[] { "Company" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(LoginCompanyRequest), Description = "Request body containing company information.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(object), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Company","related","/LoginCompany","POST",typeof(LoginCompanyRequest))]
    public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {

        return await Execute(req, async (LoginCompanyRequest request) =>
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
                return new OkObjectResult(new { Token = token, _links=HateoasScheme.Instance.GetLinks("Company")});
            }
            return new OkResult();
        });
    }
}

