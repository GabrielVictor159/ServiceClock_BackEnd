﻿using ServiceClock_BackEnd.Api.UseCases.Appointment.RequestAppointment;
using ServiceClock_BackEnd.Api.UseCases.Client.CreateClient;
using ServiceClock_BackEnd.Application.Boundaries;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Application.UseCases.Appointment.RequestAppointment.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Client.CreateClient;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Validations;
using ServiceClock_BackEnd.Infraestructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Application.UseCases.Appointment.RequestAppointment;
public class RequestAppointmentUseCase : IRequestAppointmentUseCase
{
    private readonly ILogService logService;
    private readonly RequestAppointmentPresenter outputPort;
    private readonly ValidDomainHandler<Domain.Models.Appointment, AppointmentValidator, RequestAppointmentUseCaseRequest> validDomainHandler;

    public RequestAppointmentUseCase
        (ILogService logService, 
        RequestAppointmentPresenter outputPort, 
        ValidDomainHandler<Domain.Models.Appointment, AppointmentValidator, RequestAppointmentUseCaseRequest> validDomainHandler,
        SearchEntitiesDbAppointmentHandler searchEntitiesDbAppointmentHandler,
        SaveDomainDbHandler<Domain.Models.Appointment, RequestAppointmentUseCaseRequest> saveDomainDbHandler)
    {
        validDomainHandler
            .SetSucessor(searchEntitiesDbAppointmentHandler
            .SetSucessor(saveDomainDbHandler));

        this.logService = logService;
        this.outputPort = outputPort;
        this.validDomainHandler = validDomainHandler;
    }

    public void Execute(RequestAppointmentUseCaseRequest request)
    {
        try
        {
            this.validDomainHandler.ProcessRequest(request);
            outputPort.Standard(new() { Appointment = request.Appointment });
        }
        catch (Exception ex)
        {
            logService.logs.Add(new(LogType.ERROR, "CreateClientUseCase", $"Occurring an error: {ex.Message ?? ex.InnerException?.Message}, stacktrace: {ex.StackTrace}"));
            outputPort.Error(ex.Message!);
        }
    }
}
