﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs;
using Microsoft.OpenApi.Models;
using ServiceClock_BackEnd.Api.Filters;
using ServiceClock_BackEnd.Api.UseCases.Appointment.RequestAppointment;
using ServiceClock_BackEnd.Api.Validator.Http;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.UseCases.Appointment.RequestAppointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ServiceClock_BackEnd.Application.UseCases.Appointment.AlterStateAppointment;
using ServiceClock_BackEnd.Api.Helpers.Hateoas;

namespace ServiceClock_BackEnd.Api.UseCases.Appointment.AlterStateAppointment;
public class AlterStateAppointment : UseCaseCore
{
    private readonly IMapper mapper;
    private readonly AlterStateAppointmentPresenter presenter;
    private readonly IRepository<Domain.Models.Client> clientRepository;
    private readonly IRepository<Domain.Models.Company> companyRepository;
    private IAlterStateAppointmentUseCase useCase;
    public AlterStateAppointment
        (HttpRequestValidator httpRequestValidator,
        NotificationMiddleware middleware,
        IMapper mapper,
        AlterStateAppointmentPresenter presenter,
        IAlterStateAppointmentUseCase useCase,
        IRepository<Domain.Models.Client> clientRepository,
        IRepository<Domain.Models.Company> companyRepository)
        : base(httpRequestValidator.AddValidator(new AuthorizationValidator()), middleware)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.useCase = useCase;
        this.clientRepository = clientRepository;
        this.companyRepository = companyRepository;
    }

    [FunctionName("AlterStateAppointment")]
    [OpenApiOperation(operationId: "AlterStateAppointment", tags: new[] { "Appointment" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(AlterStateAppointmentRequest), Description = "Request body containing service information.")]
    [OpenApiSecurity("bearer",
                     SecuritySchemeType.ApiKey,
                     In = OpenApiSecurityLocationType.Query,
                     Name = "code")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(AlterStateAppointmentResponse), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Appointment", "update", "/AlterStateAppointment", "POST", typeof(AlterStateAppointmentRequest))]
    public async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        return await Execute(req, async (AlterStateAppointmentRequest request) =>
        {
            request.UserId = Guid.Parse(httpRequestValidator.Claims.Where(e => e.Type == "User_Id").First().Value);
            request.UserType = httpRequestValidator.Claims.Where(e => e.Type == "User_Rule").First().Value;
            Domain.Models.Client? client = null;
            Domain.Models.Company? company = null;
            if (request.UserType == "Client")
            {
                client = this.clientRepository.Find(e => e.Id == request.UserId).FirstOrDefault();
            }
            else
            {
                company = this.companyRepository.Find(e => e.Id == request.UserId).FirstOrDefault();
            }
            if (request != null && (client != null || company!=null))
            {
                var requestUseCase = this.mapper.Map<AlterStateAppointmentUseCaseRequest>(request);
                this.useCase.Execute(requestUseCase);
            }
            return this.presenter.ViewModel;
        });
    }
}
