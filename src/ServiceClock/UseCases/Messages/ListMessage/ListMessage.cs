
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs;
using Microsoft.OpenApi.Models;
using ServiceClock_BackEnd.Validator.Http;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using System.Net;
using ServiceClock_BackEnd.Filters;
using ServiceClock_BackEnd.Helpers.Hateoas;

namespace ServiceClock_BackEnd.UseCases.Messages.ListMessage;

public class ListMessage : UseCaseCore
{
    private readonly IRepository<Domain.Models.Message> repository;
    private readonly IRepository<Domain.Models.Client> repositoryClient;
    public ListMessage
        (HttpRequestValidator httpRequestValidator, 
        NotificationMiddleware middleware,
        IRepository<Domain.Models.Message> repository,
        IRepository<Domain.Models.Client> repositoryClient) 
        : base(httpRequestValidator.AddValidator(new AuthorizationValidator()), middleware)
    {
        this.repository = repository;
        this.repositoryClient = repositoryClient;
    }
    [FunctionName("ListMessage")]
    [OpenApiOperation(operationId: "ListMessage", tags: new[] { "Message" })]
    [OpenApiSecurity("bearer",
                     SecuritySchemeType.ApiKey,
                     In = OpenApiSecurityLocationType.Query,
                     Name = "code")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ListMessageRequest), Description = "Request body containing company information.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ListMessageRequest), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Message", "search", "/ListMessage", "Post", typeof(ListMessageRequest))]
    public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        return await Execute(req, async (ListMessageRequest request) =>
        {
            var UserId = Guid.Parse(httpRequestValidator.Claims.Where(e => e.Type == "User_Id").First().Value);
            var UserType = httpRequestValidator.Claims.Where(e => e.Type == "User_Rule").First().Value;

            if (UserType == "Client")
            {
                request.ClientId = UserId;
            }
            if(UserType =="Company")
            {
                request.CompanyId = UserId;
            }

            var client = repositoryClient.Find(e=>e.Id==request.ClientId && e.Active ==true).FirstOrDefault();
            if(client == null)
            {
                return new BadRequestObjectResult("Cliente não encontrado");
            }
            return new OkObjectResult(new 
            {
                Messages =
                this.repository
                .Find(e=>e.ClientId==request.ClientId && e.CompanyId==request.CompanyId && (request.MinDate==null?true:e.CreateAt>request.MinDate) && e.Active==true)
                .OrderByDescending(e=>e.CreateAt)
                .Select(e=> new
                {
                    Id = e.Id,
                    Type = e.Type,
                    ClientId = e.ClientId,
                    CompanyId = e.CompanyId,
                    CreatedBy = e.CreatedBy,
                    MessageContent = e.MessageContent,
                    CreateAt = e.CreateAt,
                }),

                _links = HateoasScheme.Instance.GetLinks("Message")
            
            });
        });
    }

}

