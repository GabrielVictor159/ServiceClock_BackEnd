
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs;
using Microsoft.OpenApi.Models;
using ServiceClock_BackEnd.Api.Filters;
using ServiceClock_BackEnd.Api.Helpers.Hateoas;
using ServiceClock_BackEnd.Api.UseCases.Messages.CreateMessage;
using ServiceClock_BackEnd.Api.Validator.Http;
using ServiceClock_BackEnd.Application.UseCases.Messages.CreateMessage;
using System.Net;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Application.Interfaces.Services;

namespace ServiceClock_BackEnd.Api.UseCases.Messages.DeleteMessage;

public class DeleteMessage : UseCaseCore
{
    private readonly IRepository<Domain.Models.Message> repository;
    private readonly IBlobService blobService;
    public DeleteMessage
        (HttpRequestValidator httpRequestValidator, 
        NotificationMiddleware middleware,
        IRepository<Domain.Models.Message> repository,
        IBlobService blobService) 
        : base(httpRequestValidator.AddValidator(new AuthorizationValidator()), middleware)
    {
        this.repository = repository;
        this.blobService = blobService;
    }

    [FunctionName("DeleteMessage")]
    [OpenApiOperation(operationId: "DeleteMessage", tags: new[] { "Message" })]
    [OpenApiSecurity("bearer",
                     SecuritySchemeType.ApiKey,
                     In = OpenApiSecurityLocationType.Query,
                     Name = "code")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(DeleteMessageRequest), Description = "Request body containing company information.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DeleteMessageRequest), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Message", "delete", "/DeleteMessage", "POST", typeof(DeleteMessageRequest))]
    public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        return await Execute(req, async (DeleteMessageRequest request) =>
        {
            var UserId = Guid.Parse(httpRequestValidator.Claims.Where(e => e.Type == "User_Id").First().Value);
            var message = this.repository.Find(e=>e.Id==request.MessageId).FirstOrDefault();
            if(message == null)
            {
                return new BadRequestObjectResult("Message not found");
            }
            if(message.CreatedBy!=UserId)
            {
                return new ForbidResult("Você não tem permissão para excluir essa mensagem");
            }
            if(message.Type!=MessageType.Txt)
            {
                blobService.MoveBlobToPrivateContainer(message.MessageContent);
            }
            message.Active = false;
            this.repository.Update(message);

            return new OkResult();
        });
    }
}

