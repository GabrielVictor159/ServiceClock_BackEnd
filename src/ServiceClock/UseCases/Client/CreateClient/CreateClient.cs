﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs;
using ServiceClock_BackEnd.Validator.Http;
using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using ServiceClock_BackEnd.Application.UseCases.Client.CreateClient;
using ServiceClock_BackEnd.UseCases;
using ServiceClock_BackEnd.Filters;
using ServiceClock_BackEnd.Helpers.Hateoas;
using ServiceClock_BackEnd.Application.Boundaries.Appointment;
using ServiceClock_BackEnd_Application.Interfaces;
using ServiceClock_BackEnd.Application.Boundaries.Client;

namespace ServiceClock_BackEnd.UseCases.Client.CreateClient;
public class CreateClient : UseCaseCore
{
    private readonly IMapper mapper;
    private readonly IOutputPort<CreateClientBoundarie> presenter;
    private ICreateClientUseCase useCase;
    public CreateClient
        (HttpRequestValidator httpRequestValidator, 
        NotificationMiddleware middleware,
        IOutputPort<CreateClientBoundarie> createClientPresenter,
        ICreateClientUseCase useCase,
        IMapper mapper) 
        : base(httpRequestValidator.AddValidator(new AuthorizationValidator()), middleware)
    {
        this.mapper = mapper;
        this.presenter = createClientPresenter;
        this.useCase = useCase;
    }

    [FunctionName("CreateClient")]
    [OpenApiOperation(operationId: "CreateClient", tags: new[] { "Client" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateClientRequest), Description = "Request body containing company information.")]
    [OpenApiSecurity("bearer",
                     SecuritySchemeType.ApiKey,
                     In = OpenApiSecurityLocationType.Query,
                     Name = "code")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CreateClientResponse), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Client","create","/CreateClient","POST",typeof(CreateClientRequest))]
    public async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        return await Execute(req, async (CreateClientRequest request) =>
        {
            if(httpRequestValidator.Claims.Where(e => e.Type == "User_Rule").First().Value != "Company")
            {
                return new ForbidResult("Você não tem permissão para criar um usuario");
            }
            if (request != null)
            {
                var requestUseCase = this.mapper.Map<CreateClientUseCaseRequest>(request);
                if (requestUseCase.Client != null)
                {
                    requestUseCase.Client.CompanyId = Guid.Parse(httpRequestValidator.Claims.Where(e => e.Type == "User_Id").First().Value);
                    this.useCase.Execute(requestUseCase);
                }
            }
            return this.presenter.ViewModel;
        });
    }




}
