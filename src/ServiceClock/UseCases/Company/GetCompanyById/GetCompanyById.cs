
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs;
using ServiceClock_BackEnd.Validator.Http;
using System.Net;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Filters;
using ServiceClock_BackEnd.Helpers.Hateoas;

namespace ServiceClock_BackEnd.UseCases.Company.GetCompanyById;

public class GetCompanyById : UseCaseCore
{
    private readonly IRepository<Domain.Models.Company> repository;
    public GetCompanyById
        (HttpRequestValidator httpRequestValidator, 
        NotificationMiddleware middleware,
        IRepository<Domain.Models.Company> repository) 
        : base(httpRequestValidator, middleware)
    {
        this.repository = repository;
    }

    [FunctionName("GetCompanyById")]
    [OpenApiOperation(operationId: "GetCompanyById", tags: new[] { "Company" })]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Company","search","/GetCompany/{id}","GET")]
    public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetCompany/{id}")] HttpRequest req,
            string id)
    {
        Guid.TryParse(id, out Guid companyId);
        if (companyId == Guid.Empty)
        {
            return new BadRequestObjectResult("Invalid Id");
        }
        var result = this.repository.Find(e=>e.Id==companyId)
        .Select(e => new
        {
            Id = e.Id, Password = e.Password, Name = e.Name, RegistrationNumber = e.RegistrationNumber, Address = e.Address, City = e.City, State = e.State,
            Country = e.Country, PostalCode = e.PostalCode, PhoneNumber = e.PhoneNumber, Email = e.Email, Image = e.CompanyImage
        }).FirstOrDefault();

        if (result == null)
        {
            return new BadRequestObjectResult("Company not found");
        }

        return new OkObjectResult(new { Company = result, _links = HateoasScheme.Instance.GetLinks("Company") });

    }
}

