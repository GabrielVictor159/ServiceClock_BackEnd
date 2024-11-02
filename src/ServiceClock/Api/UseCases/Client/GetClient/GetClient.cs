
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using ServiceClock_BackEnd.Api.Filters;
using ServiceClock_BackEnd.Api.Validator.Http;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using System.Net;
using ServiceClock_BackEnd.Api.UseCases.Company.GetCompany;
using ServiceClock_BackEnd.Api.Helpers.Hateoas;

namespace ServiceClock_BackEnd.Api.UseCases.Client.GetClient;

public class GetClient : UseCaseCore
{
    private readonly IMapper mapper;
    private readonly IRepository<Domain.Models.Client> repository;
    public GetClient
        (HttpRequestValidator httpRequestValidator, 
        NotificationMiddleware middleware,
        IMapper mapper,
        IRepository<Domain.Models.Client> repository) 
        : base(httpRequestValidator.AddValidator(new AuthorizationValidator()), middleware)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    [FunctionName("GetClient")]
    [OpenApiOperation(operationId: "GetClient", tags: new[] { "Client" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(GetClientRequest), Description = "Request body containing company information.")]
    [OpenApiSecurity("bearer",
                     SecuritySchemeType.ApiKey,
                     In = OpenApiSecurityLocationType.Query,
                     Name = "code")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Client","search","/GetClient","POST", typeof(GetClientRequest))]
    public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        return await Execute(req, async (GetClientRequest request) =>
        {
            var UserId = Guid.Parse(httpRequestValidator.Claims.Where(e => e.Type == "User_Id").First().Value);
            var UserType = httpRequestValidator.Claims.Where(e => e.Type == "User_Rule").First().Value;
            if (UserType == "Client")
            {
                return new ForbidResult("Você não tem permissão para listar os clientes");
            }

            if (request != null)
            {
                var result = this.repository.Find(e =>
                        e.Active == true &&
                        (request.Id == Guid.Empty || e.Id == request.Id) &&
                        e.CompanyId == UserId &&
                        e.Name.ToLower().Contains(request.Name.ToLower()) &&
                        e.PhoneNumber.ToLower().Contains(request.PhoneNumber.ToLower()) &&
                        e.Address.ToLower().Contains(request.Address.ToLower()) &&
                        e.City.ToLower().Contains(request.City.ToLower()) &&
                        e.State.ToLower().Contains(request.State.ToLower()) &&
                        e.Country.ToLower().Contains(request.Country.ToLower()) &&
                        e.PostalCode.ToLower().Contains(request.PostalCode.ToLower()) &&
                        e.Email.ToLower().Contains(request.Email.ToLower()) &&
                        e.Active == true
                    , request.IndexPage, ((int)request.PageSize))
                    .Select(e => new
                    {
                        Id=e.Id, Name = e.Name, Address = e.Address, City = e.City, State = e.State,
                        Country = e.Country, PostalCode = e.PostalCode, PhoneNumber = e.PhoneNumber,
                        Email = e.Email, BirthDate = e.BirthDate, CreatedAt = e.CreatedAt,
                        Image = e.ClientImage
                    });
                return new OkObjectResult(
                    new {Clients = result, _links = HateoasScheme.Instance.GetLinks("Client")}
                );
            }
            return new OkResult();
        });
    }
}

