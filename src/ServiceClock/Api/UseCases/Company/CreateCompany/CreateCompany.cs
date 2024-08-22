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

namespace ServiceClock_BackEnd.Api.UseCases.Company.CreateCompany
{
    public class CreateCompany : UseCaseCore<CreateCompanyRequest>
    {
        private readonly ILogger<CreateCompany> logger;

        public CreateCompany
            (HttpRequestValidator httpRequestValidator, 
            NotificationMiddleware middleware,
            ILogger<CreateCompany> logger) 
            : base(httpRequestValidator, middleware)
        {
            this.logger = logger;
        }

        [FunctionName("CreateCompany")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
        {
            return await Execute(req,async (CreateCompanyRequest request) =>
            {
                
                return new OkResult();
            });
        }
    }
}

