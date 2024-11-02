
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;

namespace ServiceClock_BackEnd.Application.UseCases.Client.DeleteClient.Handlers;

public class GetAppointmentsAndMessagesHandler : Handler<DeleteClientUseCaseRequest>
{
    private readonly IRepository<Domain.Models.Appointment> appointmentRepository;
    private readonly IRepository<Domain.Models.Message> messageRepository;
    public GetAppointmentsAndMessagesHandler
        (ILogService logService,
        IRepository<Domain.Models.Appointment> appointmentRepository,
        IRepository<Domain.Models.Message> messageRepository) 
        : base(logService)
    {
        this.appointmentRepository = appointmentRepository;
        this.messageRepository = messageRepository;
    }

    public override void ProcessRequest(DeleteClientUseCaseRequest request)
    {
        request.Appointments = this.appointmentRepository.Find(e => e.ClientId == request.Client.Id).ToList();
        request.Messages = this.messageRepository.Find(e => e.ClientId == request.Client.Id).ToList();
        sucessor?.ProcessRequest(request);
    }
}

