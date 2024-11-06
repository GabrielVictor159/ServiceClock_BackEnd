
using ServiceClock_BackEnd.Application.Boundaries;
using ServiceClock_BackEnd.Application.Boundaries.Appointment;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Application.UseCases.Appointment.AlterStateAppointment.Handlers;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd_Application.Interfaces;

namespace ServiceClock_BackEnd.Application.UseCases.Appointment.AlterStateAppointment;
public class AlterStateAppointmentUseCase : IAlterStateAppointmentUseCase
{
    private readonly ILogService logService;
    private readonly IOutputPort<AlterStateAppointmentBoundarie> outputPort;
    private readonly ValidProcessAlterStateAppointmentHandler handler;

    public AlterStateAppointmentUseCase
        (ILogService logService,
        IOutputPort<AlterStateAppointmentBoundarie> presenter, 
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
            outputPort.Standard(new() { Appointment = request.Appointment, Status=request.Status });
        }
        catch (Exception ex)
        {
            logService.logs.Add(new(LogType.ERROR, "CreateClientUseCase", $"Occurring an error: {ex.Message ?? ex.InnerException?.Message}, stacktrace: {ex.StackTrace}"));
            outputPort.Error(ex.Message!);
        }
    }
}
