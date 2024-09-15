using AutoMapper;
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

namespace ServiceClock_BackEnd.Api.UseCases.Appointment.AlterStateAppointment;
public class AlterStateAppointment : UseCaseCore
{
    private readonly IMapper mapper;
    private readonly AlterStateAppointmentPresenter presenter;
    private readonly IRepository<Domain.Models.Client> clientReposiotory;
    private IAlterStateAppointmentUseCase useCase;
    public AlterStateAppointment
        (HttpRequestValidator httpRequestValidator,
        NotificationMiddleware middleware,
        IMapper mapper,
        AlterStateAppointmentPresenter presenter,
        IAlterStateAppointmentUseCase useCase,
        IRepository<Domain.Models.Client> clientReposiotory)
        : base(httpRequestValidator.AddValidator(new AuthorizationValidator()), middleware)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.useCase = useCase;
        this.clientReposiotory = clientReposiotory;
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
    public async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        return await Execute(req, async (AlterStateAppointmentRequest request) =>
        {
            var client = this.clientReposiotory.Find(e => e.Id == Guid.Parse(httpRequestValidator.Claims.Where(e => e.Type == "User_Id").First().Value)).FirstOrDefault();
            if (request != null && client != null)
            {
                request.UserId = Guid.Parse(httpRequestValidator.Claims.Where(e => e.Type == "User_Id").First().Value);
                request.UserType = httpRequestValidator.Claims.Where(e => e.Type == "User_Rule").First().Value;
                var requestUseCase = this.mapper.Map<AlterStateAppointmentUseCaseRequest>(request);
                this.useCase.Execute(requestUseCase);
            }
            return this.presenter.ViewModel;
        });
    }
}
