﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs;
using Microsoft.OpenApi.Models;
using ServiceClock_BackEnd.Api.Filters;
using ServiceClock_BackEnd.Api.UseCases.Services.CreateService;
using ServiceClock_BackEnd.Api.Validator.Http;
using ServiceClock_BackEnd.Application.UseCases.Services.CreateService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ServiceClock_BackEnd.Api.UseCases.Client.CreateClient;
using ServiceClock_BackEnd.Application.UseCases.Client.CreateClient;
using ServiceClock_BackEnd.Application.UseCases.Appointment.RequestAppointment;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Api.Helpers.Hateoas;

namespace ServiceClock_BackEnd.Api.UseCases.Appointment.RequestAppointment;
public class RequestAppointment : UseCaseCore
{
    private readonly IMapper mapper;
    private readonly RequestAppointmentPresenter presenter;
    private readonly IRepository<Domain.Models.Client> clientReposiotory;
    private IRequestAppointmentUseCase useCase;
    public RequestAppointment
        (HttpRequestValidator httpRequestValidator, 
        NotificationMiddleware middleware,
        IMapper mapper,
        RequestAppointmentPresenter presenter,
        IRequestAppointmentUseCase useCase,
        IRepository<Domain.Models.Client> clientReposiotory) 
        : base(httpRequestValidator.AddValidator(new AuthorizationValidator()), middleware)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.useCase = useCase;
        this.clientReposiotory = clientReposiotory;
    }

    [FunctionName("RequestAppointment")]
    [OpenApiOperation(operationId: "RequestAppointment", tags: new[] { "Appointment" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(RequestAppointmentRequest), Description = "Request body containing service information.")]
    [OpenApiSecurity("bearer",
                     SecuritySchemeType.ApiKey,
                     In = OpenApiSecurityLocationType.Query,
                     Name = "code")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(RequestAppointmentResponse), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Appointment","create","/RequestAppointment","POST", typeof(RequestAppointmentRequest))]
    public async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        return await Execute(req, async (RequestAppointmentRequest request) =>
        {
            if (httpRequestValidator.Claims.Where(e => e.Type == "User_Rule").First().Value != "Client")
            {

                return new UnauthorizedObjectResult("The user does not have permission to perform this action.");
            }
            var userId = Guid.Parse(httpRequestValidator.Claims.Where(e => e.Type == "User_Id").First().Value);
            var client = this.clientReposiotory.Find(e => e.Id == userId).FirstOrDefault();
            if (request != null && client!=null)
            {
                request.clientId = client.Id;
                var requestUseCase = this.mapper.Map<RequestAppointmentUseCaseRequest>(request);
                requestUseCase.Client = client;
                this.useCase.Execute(requestUseCase);
            }
            return this.presenter.ViewModel;
        });
    }
}
