
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ServiceClock_BackEnd.Validator.Http;
using AutoMapper;
using ServiceClock_BackEnd.Application.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.Filters;
using ServiceClock_BackEnd.Helpers.Hateoas;
using ServiceClock_BackEnd_Application.Interfaces;
using ServiceClock_BackEnd.Application.Boundaries.Company;
#pragma warning disable CS1998
namespace ServiceClock_BackEnd.UseCases.Company.CreateCompany
{
    public class CreateCompany : UseCaseCore
    {
        private readonly IMapper mapper;
        private readonly IOutputPort<CreateCompanyBoundarie> presenter;
        private readonly ICreateCompanyUseCase useCase;

        public CreateCompany
            (HttpRequestValidator httpRequestValidator,
            NotificationMiddleware middleware,ILogger<CreateCompany> logger, 
            IMapper mapper,
            IOutputPort<CreateCompanyBoundarie> presenter, 
            ICreateCompanyUseCase useCase)
            : base(httpRequestValidator, middleware)
        {
            this.mapper = mapper;
            this.presenter = presenter;
            this.useCase = useCase;
        }

        [FunctionName("CreateCompany")]
        [OpenApiOperation(operationId: "CreateCompany", tags: new[] { "Company" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateCompanyRequest), Description = "Request body containing company information.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CreateCompanyResponse), Description = "The OK response with the created company details.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
        [Hateoas("Company", "create", "/CreateCompany", "POST", typeof(CreateCompanyRequest))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
        {
            return await Execute(req,async (CreateCompanyRequest request) =>
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
}

#pragma warning restore CS1998

