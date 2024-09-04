using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs;
using ServiceClock_BackEnd.Api.Filters;
using ServiceClock_BackEnd.Api.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.Api.Validator.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ServiceClock_BackEnd.Application.UseCases.Company.CreateCompany;

namespace ServiceClock_BackEnd.Api.UseCases.Client.CreateClient;
public class CreateClient : UseCaseCore
{
    private readonly IMapper mapper;
    private readonly CreateClientPresenter presenter;
    private ICreateCompanyUseCase useCase;
    public CreateClient
        (HttpRequestValidator httpRequestValidator, 
        NotificationMiddleware middleware,
        CreateClientPresenter createClientPresenter,
        IMapper mapper) 
        : base(httpRequestValidator, middleware)
    {
        this.mapper = mapper;
        this.presenter = createClientPresenter;
    }

    [FunctionName("CreateClient")]
    [OpenApiOperation(operationId: "CreateClient", tags: new[] { "Company" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateClientRequest), Description = "Request body containing company information.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CreateClientResponse), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    public async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        return await Execute(req, async (CreateClientRequest request) =>
        {
            if (request != null)
            {
                var requestUseCase = this.mapper.Map<CreateCompanyUseCaseRequest>(request);
                this.useCase.Execute(requestUseCase);
            }
            return this.presenter.ViewModel;
        });
    }




}
