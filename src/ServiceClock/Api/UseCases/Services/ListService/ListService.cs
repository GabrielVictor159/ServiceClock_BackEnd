
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs;
using Microsoft.OpenApi.Models;
using ServiceClock_BackEnd.Api.Filters;
using ServiceClock_BackEnd.Api.UseCases.Client.GetClient;
using ServiceClock_BackEnd.Api.Validator.Http;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using System.Net;
using ServiceClock_BackEnd.Api.Helpers.Hateoas;

namespace ServiceClock_BackEnd.Api.UseCases.Services.ListService;

public class ListService : UseCaseCore
{
    private readonly IMapper mapper;
    private readonly IRepository<Domain.Models.Client> clientRepository;
    private readonly IRepository<Domain.Models.Service> serviceRepository;
    public ListService
        (HttpRequestValidator httpRequestValidator,
        NotificationMiddleware middleware,
        IMapper mapper,
        IRepository<Domain.Models.Client> clientRepository,
        IRepository<Domain.Models.Service> serviceRepository)
        : base(httpRequestValidator.AddValidator(new AuthorizationValidator()), middleware)
    {
        this.mapper = mapper;
        this.clientRepository = clientRepository;
        this.serviceRepository = serviceRepository;
    }

    [FunctionName("ListService")]
    [OpenApiOperation(operationId: "ListService", tags: new[] { "Service" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ListServiceRequest), Description = "Request body containing company information.")]
    [OpenApiSecurity("bearer",
                     SecuritySchemeType.ApiKey,
                     In = OpenApiSecurityLocationType.Query,
                     Name = "code")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Service", "search", "/ListService", "POST", typeof(ListServiceRequest))]
    public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        return await Execute(req, async (ListServiceRequest request) =>
        {
            var rule = httpRequestValidator.Claims.Where(e => e.Type == "User_Rule").First().Value;
            var id = httpRequestValidator.Claims.Where(e => e.Type == "User_Id").First().Value;
            var companyId = Guid.Empty;
            if (rule != "Company")
            {
                var client = this.clientRepository.Find(e => e.Id == Guid.Parse(id) && e.Active==true).FirstOrDefault();
                if(client == null)
                {
                    return new UnauthorizedResult();
                }
                companyId = client.CompanyId??Guid.Empty;

            }
            else
            {
                companyId = Guid.Parse(id);
            }
            if (request != null)
            {
                return new OkObjectResult(new
                { Services =
                    this.serviceRepository.Find(e =>
                        (request.Id == Guid.Empty || e.Id == request.Id) &&
                        e.Name.ToLower().Contains(request.Name.ToLower()) &&
                        e.Description.ToLower().Contains(request.Description.ToLower()) &&
                        e.Address.ToLower().Contains(request.Address.ToLower()) &&
                        e.City.ToLower().Contains(request.City.ToLower()) &&
                        e.State.ToLower().Contains(request.State.ToLower()) &&
                        e.Country.ToLower().Contains(request.Country.ToLower()) &&
                        e.PostalCode.ToLower().Contains(request.PostalCode.ToLower()) &&
                        e.CompanyId == companyId
                    , request.IndexPage, ((int)request.PageSize))
                    .Select(e => new
                    {
                        Id = e.Id, Name = e.Name, Description = e.Description, Address = e.Address,
                        City = e.City, State = e.State, Country = e.Country, PostalCode = e.PostalCode,
                    })
                , _links = HateoasScheme.Instance.GetLinks("Service")});
            }
            return new OkResult();
        });
    }
}

