using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs;
using ServiceClock_BackEnd.Validator.Http;
using ServiceClock_BackEnd.Application.UseCases.Client.PatchClient;
using System.Net;
using ServiceClock_BackEnd.Filters;
using ServiceClock_BackEnd.Helpers.Hateoas;
using ServiceClock_BackEnd_Application.Interfaces;
using ServiceClock_BackEnd.Application.Boundaries.Client;

namespace ServiceClock_BackEnd.UseCases.Client.PatchClient;

public class PatchClient : UseCaseCore
{
    private readonly IMapper mapper;
    private readonly IOutputPort<PatchClientBoundarie> presenter;
    private readonly IPatchClientUseCase useCase;
    public PatchClient
        (HttpRequestValidator httpRequestValidator, 
        NotificationMiddleware middleware,
        IMapper mapper,
        IOutputPort<PatchClientBoundarie> presenter,
        IPatchClientUseCase useCase) 
        : base(httpRequestValidator.AddValidator(new AuthorizationValidator()), middleware)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.useCase = useCase;
    }

    [FunctionName("PatchClient")]
    [OpenApiOperation(operationId: "PatchClient", tags: new[] { "Client" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(PatchClientRequest), Description = "Request body containing company information.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(PatchClientResponse), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Client","update","/PatchClient","PATCH", typeof(PatchClientRequest))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = null)] HttpRequest req)
    {
        return await Execute(req, async (PatchClientRequest request) =>
        {
            if (request != null)
            {
                var requestUseCase = this.mapper.Map<PatchClientUseCaseRequest>(request);
                requestUseCase.Client.Id = Guid.Parse(httpRequestValidator.Claims.Where(e => e.Type == "User_Id").First().Value);
                this.useCase.Execute(requestUseCase);
            }
            return this.presenter.ViewModel;
        });
    }
}

