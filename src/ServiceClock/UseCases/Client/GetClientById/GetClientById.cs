
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs;
using ServiceClock_BackEnd.Validator.Http;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using System.Net;
using ServiceClock_BackEnd.Filters;
using ServiceClock_BackEnd.Helpers.Hateoas;

namespace ServiceClock_BackEnd.UseCases.Client.GetClientById;

public class GetClientById : UseCaseCore
{
    private readonly IRepository<Domain.Models.Client> repository;
    public GetClientById
        (HttpRequestValidator httpRequestValidator, 
        NotificationMiddleware middleware,
        IRepository<Domain.Models.Client> repository) 
        : base(httpRequestValidator, middleware)
    {
        this.repository = repository;
    }
    [FunctionName("GetClientById")]
    [OpenApiOperation(operationId: "GetClientById", tags: new[] { "Client" })]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Client","search","/GetClient/{id}","GET")]
    public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetClient/{id}")] HttpRequest req,
            string id)
    {
        Guid.TryParse(id, out Guid companyId);
        if (companyId == Guid.Empty)
        {
            return new BadRequestObjectResult("Invalid Id");
        }
        var result = this.repository.Find(e => e.Id == companyId && e.Active == true)
        .Select(e => new
        {
            Id = e.Id, Password = e.Password, Name = e.Name, Address = e.Address, City = e.City, State = e.State,
            Country = e.Country, PostalCode = e.PostalCode, PhoneNumber = e.PhoneNumber, Email = e.Email, Image = e.ClientImage,
            BirthDate = e.BirthDate, CreatedAt = e.CreatedAt,
        }).FirstOrDefault();

        if (result == null)
        {
            return new BadRequestObjectResult("Client not found");
        }

        return new OkObjectResult(new { Client = result, _links = HateoasScheme.Instance.GetLinks("Client") });

    }

}

