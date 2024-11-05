
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs;
using ServiceClock_BackEnd.Validator.Http;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using ServiceClock_BackEnd.Filters;
using ServiceClock_BackEnd.Helpers.Hateoas;

namespace ServiceClock_BackEnd.UseCases.Company.GetCompany;

public class GetCompany : UseCaseCore
{
    private readonly IMapper mapper;
    private readonly IRepository<Domain.Models.Company> repository;
    public GetCompany
        (HttpRequestValidator httpRequestValidator,
        NotificationMiddleware middleware,
        IMapper mapper,
        IRepository<Domain.Models.Company> repository)
        : base(httpRequestValidator.AddValidator(new AuthorizationValidator()), middleware)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    [FunctionName("GetCompany")]
    [OpenApiOperation(operationId: "GetCompany", tags: new[] { "Company" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(GetCompanyRequest), Description = "Request body containing company information.")]
    [OpenApiSecurity("bearer",
                     SecuritySchemeType.ApiKey,
                     In = OpenApiSecurityLocationType.Query,
                     Name = "code")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Company", "search", "/GetCompany", "POST", typeof(GetCompanyRequest))]
    public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        return await Execute(req, async (GetCompanyRequest request) =>
        {
            if (request != null)
            {

                return new OkObjectResult(new { Companies =
                    this.repository.Find(e =>
                        (request.Id == Guid.Empty || e.Id == request.Id) &&
                        e.Name.ToLower().Contains(request.Name.ToLower()) &&
                        e.RegistrationNumber.ToLower().Contains(request.RegistrationNumber.ToLower()) &&
                        e.Address.ToLower().Contains(request.Address.ToLower()) &&
                        e.City.ToLower().Contains(request.City.ToLower()) &&
                        e.State.ToLower().Contains(request.State.ToLower()) &&
                        e.Country.ToLower().Contains(request.Country.ToLower()) &&
                        e.PostalCode.ToLower().Contains(request.PostalCode.ToLower()) &&
                        e.PhoneNumber.ToLower().Contains(request.PhoneNumber.ToLower()) &&
                        e.Email.ToLower().Contains(request.Email.ToLower())
                    , request.IndexPage, ((int)request.PageSize))
                    .Select(e => new
                    {
                        Name = e.Name, RegistrationNumber = e.RegistrationNumber, Address = e.Address, City = e.City, State = e.State,
                        Country = e.Country, PostalCode = e.PostalCode, PhoneNumber = e.PhoneNumber, Email = e.Email, Image = e.CompanyImage
                    }), _links = HateoasScheme.Instance.GetLinks("Company") }
                );
            }
            return new OkResult();
        });
    }
}

