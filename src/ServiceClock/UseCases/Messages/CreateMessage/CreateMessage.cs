﻿
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs;
using ServiceClock_BackEnd.Validator.Http;
using ServiceClock_BackEnd.Application.UseCases.Messages.CreateMessage;
using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Filters;
using ServiceClock_BackEnd.Helpers.Hateoas;
using ServiceClock_BackEnd.Application.Boundaries.Company;
using ServiceClock_BackEnd_Application.Interfaces;
using ServiceClock_BackEnd.Application.Boundaries.Messages;

namespace ServiceClock_BackEnd.UseCases.Messages.CreateMessage;

public class CreateMessage : UseCaseCore
{
    private readonly IMapper mapper;
    private readonly IOutputPort<CreateMessageBoundarie> presenter;
    private readonly ICreateMessageUseCase useCase;
    private readonly IRepository<Domain.Models.Client> clientRepository;
    public CreateMessage
        (HttpRequestValidator httpRequestValidator,
        NotificationMiddleware middleware,
        IMapper mapper,
        IOutputPort<CreateMessageBoundarie> presenter, 
        ICreateMessageUseCase useCase,
        IRepository<Domain.Models.Client> clientRepository) 
        : base(httpRequestValidator.AddValidator(new AuthorizationValidator()), middleware)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.useCase = useCase;
        this.clientRepository = clientRepository;
    }

    [FunctionName("CreateMessage")]
    [OpenApiOperation(operationId: "CreateMessage", tags: new[] { "Message" })]
    [OpenApiSecurity("bearer",
                     SecuritySchemeType.ApiKey,
                     In = OpenApiSecurityLocationType.Query,
                     Name = "code")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateMessageRequest), Description = "Request body containing company information.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CreateMessageResponse), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Message","create","/CreateMessage","POST",typeof(CreateMessageRequest))]
    public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        return await Execute(req, async (CreateMessageRequest request) =>
        {
            if (request != null)
            {
                var UserId = Guid.Parse(httpRequestValidator.Claims.Where(e => e.Type == "User_Id").First().Value);
                var UserType = httpRequestValidator.Claims.Where(e => e.Type == "User_Rule").First().Value;

                request.CreatedBy = UserId;

                if (UserType == "Client")
                {
                   var client = this.clientRepository.Find(e => e.Id == UserId && e.Active==true).FirstOrDefault();
                   if(client == null)
                   {
                       return new BadRequestObjectResult("Client not found");
                   }
                   request.ClientId = client.Id;
                   request.CompanyId = client.CompanyId ?? request.CompanyId;
                }

                if(UserType == "Company")
                {
                    request.CompanyId = UserId;
                    var client = this.clientRepository.Find(e => e.Id == request.ClientId && e.CompanyId==request.CompanyId && e.Active == true).FirstOrDefault();
                    if(client == null)
                    {
                        return new BadRequestObjectResult("Client not found");
                    }
                }

                var requestUseCase = this.mapper.Map<CreateMessageUseCaseRequest>(request);
                this.useCase.Execute(requestUseCase);
            }
            return this.presenter.ViewModel;
        });
    }
}

