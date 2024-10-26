
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs;
using Microsoft.OpenApi.Models;
using ServiceClock_BackEnd.Api.Filters;
using ServiceClock_BackEnd.Api.Helpers.Hateoas;
using ServiceClock_BackEnd.Api.UseCases.Services.CreateService;
using ServiceClock_BackEnd.Api.Validator.Http;
using ServiceClock_BackEnd.Application.UseCases.Services.CreateService;
using System.Net;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Application.UseCases.Services.EditService;

namespace ServiceClock_BackEnd.Api.UseCases.Services.EditService;

public class EditService : UseCaseCore
{
    private readonly IMapper mapper;
    private readonly EditServicePresenter presenter;
    private readonly IRepository<Service> repository;
    private IEditServiceUseCase useCase;

    public EditService
        (HttpRequestValidator httpRequestValidator,
        NotificationMiddleware middleware,
        IMapper mapper,
        EditServicePresenter presenter,
        IRepository<Service> repository,
        IEditServiceUseCase useCase)
        : base(httpRequestValidator.AddValidator(new AuthorizationValidator()), middleware)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.useCase = useCase;
        this.repository = repository;
    }

    [FunctionName("EditService")]
    [OpenApiOperation(operationId: "EditService", tags: new[] { "Service" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(EditServiceRequest), Description = "Request body containing service information.")]
    [OpenApiSecurity("bearer",
                     SecuritySchemeType.ApiKey,
                     In = OpenApiSecurityLocationType.Query,
                     Name = "code")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(EditServiceResponse), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Service", "edit", "/EditService", "PATCH", typeof(EditServiceRequest))]
    public async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = null)] HttpRequest req)
    {
        return await Execute(req, async (EditServiceRequest request) =>
        {
            var userId = Guid.Parse(httpRequestValidator.Claims.Where(e => e.Type == "User_Id").First().Value);
            if (request != null)
            {
                var serviceExisting = repository.FindSingle(e => e.Id == request.Id);
                if(serviceExisting==null)
                {
                    return new NotFoundObjectResult("Serviço não encontrado");
                }
                if(!serviceExisting.CompanyId.Equals(userId))
                {
                    return new ForbidResult();
                }

                var newService = mapper.Map(request,serviceExisting);

                if(newService == null)
                {
                    return new BadRequestObjectResult("Não foi possivel converter o request para o dominio");
                }

                var useCaseRequest = new EditServiceUseCaseRequest(newService);

                this.useCase.Execute(useCaseRequest);
            }
            return this.presenter.ViewModel;
        });
    }
}

