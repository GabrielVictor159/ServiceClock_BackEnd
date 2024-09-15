using ServiceClock_BackEnd.Api.UseCases.Appointment.AlterStateAppointment;
using ServiceClock_BackEnd.Application.Boundaries;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Application.UseCases.Appointment.AlterStateAppointment.Handlers;
using ServiceClock_BackEnd.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Application.UseCases.Appointment.AlterStateAppointment;
public class AlterStateAppointmentUseCase : IAlterStateAppointmentUseCase
{
    private readonly ILogService logService;
    private readonly AlterStateAppointmentPresenter outputPort;
    private readonly ValidProcessAlterStateAppointmentHandler handler;

    public AlterStateAppointmentUseCase
        (ILogService logService, 
        AlterStateAppointmentPresenter presenter, 
        ValidProcessAlterStateAppointmentHandler handler,
        AlterStateAppointmentHandler alterStateAppointmentHandler)
    {
        handler.SetSucessor(alterStateAppointmentHandler);

        this.logService = logService;
        this.outputPort = presenter;
        this.handler = handler;

    }

    public void Execute(AlterStateAppointmentUseCaseRequest request)
    {
        try
        {
            this.handler.ProcessRequest(request);
            outputPort.Standard(new() { Appointment = request.Appointment });
        }
        catch (Exception ex)
        {
            logService.logs.Add(new(LogType.ERROR, "CreateClientUseCase", $"Occurring an error: {ex.Message ?? ex.InnerException?.Message}, stacktrace: {ex.StackTrace}"));
            outputPort.Error(ex.Message!);
        }
    }
}
