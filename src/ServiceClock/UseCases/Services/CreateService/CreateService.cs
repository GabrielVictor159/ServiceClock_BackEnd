
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs;
using Microsoft.OpenApi.Models;
using ServiceClock_BackEnd.Validator.Http;
using ServiceClock_BackEnd.Application.UseCases.Services.CreateService;
using System.Net;
using ServiceClock_BackEnd.Filters;
using ServiceClock_BackEnd.Helpers.Hateoas;
using ServiceClock_BackEnd.Application.Boundaries.Messages;
using ServiceClock_BackEnd_Application.Interfaces;
using ServiceClock_BackEnd.Application.Boundaries.Services;

namespace ServiceClock_BackEnd.UseCases.Services.CreateService;

public class CreateService : UseCaseCore
{
    private readonly IMapper mapper;
    private readonly IOutputPort<CreateServiceBoundarie> presenter;
    private ICreateServiceUseCase useCase;
    public CreateService
        (HttpRequestValidator httpRequestValidator, 
        NotificationMiddleware middleware,
        IMapper mapper,
        IOutputPort<CreateServiceBoundarie> presenter, 
        ICreateServiceUseCase useCase) 
        : base(httpRequestValidator.AddValidator(new AuthorizationValidator()), middleware)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.useCase = useCase;
    }

    [FunctionName("CreateService")]
    [OpenApiOperation(operationId: "CreateService", tags: new[] { "Service" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateServiceRequest), Description = "Request body containing service information.")]
    [OpenApiSecurity("bearer",
                     SecuritySchemeType.ApiKey,
                     In = OpenApiSecurityLocationType.Query,
                     Name = "code")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CreateServiceResponse), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Service","create","/CreateService","POST",typeof(CreateServiceRequest))]
    public async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        return await Execute(req, async (CreateServiceRequest request) =>
        {
            if (httpRequestValidator.Claims.Where(e => e.Type == "User_Rule").First().Value != "Company")
            {
                return new ForbidResult("Você não tem permissão para criar um serviço");
            }
            if (request != null)
            {
                var requestUseCase = this.mapper.Map<CreateServiceUseCaseRequest>(request);
                if (requestUseCase.Service != null)
                {
                    requestUseCase.Service.CompanyId = Guid.Parse(httpRequestValidator.Claims.Where(e => e.Type == "User_Id").First().Value);
                    this.useCase.Execute(requestUseCase);
                }
            }
            return this.presenter.ViewModel;
        });
    }

}

