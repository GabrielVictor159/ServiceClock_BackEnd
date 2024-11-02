
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
using ServiceClock_BackEnd.Api.UseCases.Client.CreateClient;
using ServiceClock_BackEnd.Api.Validator.Http;
using ServiceClock_BackEnd.Application.UseCases.Client.CreateClient;
using System.Net;
using ServiceClock_BackEnd.Application.UseCases.Client.DeleteClient;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;

namespace ServiceClock_BackEnd.Api.UseCases.Client.DeleteClient;

public class DeleteClient : UseCaseCore
{
    private readonly IMapper mapper;
    private readonly DeleteClientPresenter presenter;
    private IDeleteClientUseCase useCase;
    private readonly IRepository<Domain.Models.Client> clientRepository;
    public DeleteClient
        (HttpRequestValidator httpRequestValidator,
        NotificationMiddleware middleware,
        DeleteClientPresenter createClientPresenter,
        IDeleteClientUseCase useCase,
        IMapper mapper,
        IRepository<Domain.Models.Client> clientRepository)
        : base(httpRequestValidator.AddValidator(new AuthorizationValidator()), middleware)
    {
        this.mapper = mapper;
        this.presenter = createClientPresenter;
        this.useCase = useCase;
        this.clientRepository = clientRepository;
    }

    [FunctionName("DeleteClient")]
    [OpenApiOperation(operationId: "DeleteClient", tags: new[] { "Client" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(DeleteClientRequest), Description = "Request body containing company information.")]
    [OpenApiSecurity("bearer",
                     SecuritySchemeType.ApiKey,
                     In = OpenApiSecurityLocationType.Query,
                     Name = "code")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DeleteClientResponse), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Client", "delete", "/DeleteClient", "POST", typeof(DeleteClientRequest))]
    public async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        return await Execute(req, async (DeleteClientRequest request) =>
        {
            var userId = Guid.Parse(httpRequestValidator.Claims.Where(e => e.Type == "User_Id").First().Value);
            var client = this.clientRepository.Find(e => e.Id == request.ClientId && e.CompanyId == userId && e.Active==true).FirstOrDefault();

            if(client==null)
            {
                return new NotFoundObjectResult("Cliente não encontrado");
            }

            var requestUseCase = new DeleteClientUseCaseRequest(client);
            this.useCase.Execute(requestUseCase);
            return this.presenter.ViewModel;
        });
    }
}

