using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ServiceClock_BackEnd.Api.Filters;
using ServiceClock_BackEnd.Api.Validator.Http;
using AutoMapper;
using ServiceClock_BackEnd.Application.UseCases.CreateCompany;
#pragma warning disable CS1998
namespace ServiceClock_BackEnd.Api.UseCases.Company.CreateCompany
{
    public class CreateCompany : UseCaseCore
    {
        private readonly IMapper mapper;
        private readonly CreateCompanyPresenter presenter;
        private readonly ICreateCompanyUseCase useCase;

        public CreateCompany
            (HttpRequestValidator httpRequestValidator,
            NotificationMiddleware middleware,ILogger<CreateCompany> logger, 
            IMapper mapper, 
            CreateCompanyPresenter presenter, 
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

