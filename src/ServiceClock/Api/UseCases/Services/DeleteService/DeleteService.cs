
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs;
using ServiceClock_BackEnd.Api.Filters;
using ServiceClock_BackEnd.Api.Validator.Http;
using System.Net;
using ServiceClock_BackEnd.Api.UseCases.Services.CreateService;
using ServiceClock_BackEnd.Application.UseCases.Services.CreateService;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using AutoMapper;
using ServiceClock_BackEnd.Application.UseCases.Services.DeleteService;
using ServiceClock_BackEnd.Api.Helpers.Hateoas;

namespace ServiceClock_BackEnd.Api.UseCases.Services.DeleteService;

public class DeleteService : UseCaseCore
{
    private readonly IMapper mapper;
    private readonly DeleteServicePresenter presenter;
    private IDeleteServiceUseCase useCase;
    public DeleteService
        (HttpRequestValidator httpRequestValidator, 
        NotificationMiddleware middleware,
        IMapper mapper,
        DeleteServicePresenter presenter,
        IDeleteServiceUseCase useCase) 
        : base(httpRequestValidator.AddValidator(new AuthorizationValidator()), middleware)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.useCase = useCase;
    }

    [FunctionName("DeleteService")]
    [OpenApiOperation(operationId: "DeleteService", tags: new[] { "Service" })]
    [OpenApiSecurity("bearer",
                     SecuritySchemeType.ApiKey,
                     In = OpenApiSecurityLocationType.Query,
                     Name = "code")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Service","delete","/DeleteService","POST",typeof(DeleteServiceRequest))]
    public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "DeleteService")] HttpRequest req)
    {
        return await Execute(req, async (DeleteServiceRequest request) =>
        {
            Guid.TryParse(httpRequestValidator.Claims.Where(e => e.Type == "User_Id").First().Value, out Guid companyId);

            if(companyId == Guid.Empty)
            {
                return new BadRequestObjectResult("Invalid token");
            }
            if (request != null)
            {
                var requestUseCase = this.mapper.Map<DeleteServiceUseCaseRequest>(request);
                requestUseCase.CompanyId = companyId;
                this.useCase.Execute(requestUseCase);
                
            }
            return this.presenter.ViewModel;
        });

    }
}

