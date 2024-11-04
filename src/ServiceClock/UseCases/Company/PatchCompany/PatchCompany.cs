
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ServiceClock_BackEnd.Api.Filters;
using ServiceClock_BackEnd.Api.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.Validator.Http
using System.Net;
using Microsoft.AspNetCore.Authorization;
using ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany;
using ServiceClock_BackEnd.Api.Helpers.Hateoas;

namespace ServiceClock_BackEnd.Api.UseCases.Company.PatchCompany;

public class PatchCompany : UseCaseCore
{
    private readonly IMapper mapper;
    private readonly PatchCompanyPresenter presenter;
    private readonly IPatchCompanyUseCase useCase;

    public PatchCompany
        (HttpRequestValidator httpRequestValidator,
        NotificationMiddleware middleware, ILogger<PatchCompany> logger,
        IMapper mapper,
        PatchCompanyPresenter presenter,
        IPatchCompanyUseCase useCase)
        : base(httpRequestValidator.AddValidator(new AuthorizationValidator()), middleware)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.useCase = useCase;
    }

    [FunctionName("PatchCompany")]
    [OpenApiOperation(operationId: "PatchCompany", tags: new[] { "Company" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(PatchCompanyRequest), Description = "Request body containing company information.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(PatchCompanyResponse), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Company", "update", "/PatchCompany", "PATCH", typeof(PatchCompanyRequest))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = null)] HttpRequest req)
    {
        return await Execute(req, async (PatchCompanyRequest request) =>
        {
            if (request != null)
            {
                var requestUseCase = this.mapper.Map<PatchCompanyUseCaseRequest>(request);
                requestUseCase.Company.Id = Guid.Parse(httpRequestValidator.Claims.Where(e => e.Type == "User_Id").First().Value);
                this.useCase.Execute(requestUseCase);
            }
            return this.presenter.ViewModel;
        });
    }
}

