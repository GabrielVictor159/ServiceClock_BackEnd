
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;

namespace ServiceClock_BackEnd.Application.UseCases.Client.DeleteClient.Handlers;

public class DeleteAppointmentsHandler : Handler<DeleteClientUseCaseRequest>
{
    private readonly IRepository<Domain.Models.Appointment> appointmentRepository;
    public DeleteAppointmentsHandler
        (ILogService logService,
        IRepository<Domain.Models.Appointment> appointmentRepository)
        : base(logService)
    {
        this.appointmentRepository = appointmentRepository;
    }

    public override void ProcessRequest(DeleteClientUseCaseRequest request)
    {
        request.AppointmentsDeleteRows = this.appointmentRepository.DeleteRange(request.Appointments);
        sucessor?.ProcessRequest(request);
    }
}

