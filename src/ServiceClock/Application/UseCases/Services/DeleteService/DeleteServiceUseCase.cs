﻿
using ServiceClock_BackEnd.Api.UseCases.Services.CreateService;
using ServiceClock_BackEnd.Api.UseCases.Services.DeleteService;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Services.CreateService;
using ServiceClock_BackEnd.Application.UseCases.Services.DeleteService.Handlers;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Domain.Validations;

namespace ServiceClock_BackEnd.Application.UseCases.Services.DeleteService;

public class DeleteServiceUseCase : IDeleteServiceUseCase
{
    private readonly DeleteServicePresenter outputPort;
    private readonly ILogService logService;
    private readonly GetServiceHandler handler;

    public DeleteServiceUseCase
        (DeleteServicePresenter outputPort,
        ILogService logService,
        GetServiceHandler handler,
        GetAppointmentsHandler getAppointmentsHandler,
        DeleteServiceHandler deleteServiceHandler)
    {
        handler
            .SetSucessor(getAppointmentsHandler
            .SetSucessor(deleteServiceHandler));

        this.outputPort = outputPort;
        this.logService = logService;
        this.handler = handler;
    }

    public void Execute(DeleteServiceUseCaseRequest request)
    {
        try
        {
            this.handler.ProcessRequest(request);
            outputPort.Standard(new() { Sucess = request.IsDeleted });
        }
        catch (Exception ex)
        {
            logService.logs.Add(new(LogType.ERROR, "CreateCompanyUseCase", $"Occurring an error: {ex.Message ?? ex.InnerException?.Message}, stacktrace: {ex.StackTrace}"));
            outputPort.Error(ex.Message!);
        }
    }
}

