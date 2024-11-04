
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Application.UseCases.Services.DeleteService.Handlers;

public class GetAppointmentsHandler : Handler<DeleteServiceUseCaseRequest>
{
    private readonly IRepository<Domain.Models.Appointment> repository;
    private readonly INotificationService notificationService;
    public GetAppointmentsHandler
        (ILogService logService,
        IRepository<Domain.Models.Appointment> repository,
        INotificationService notificationService) 
        : base(logService)
    {
        this.repository = repository;
        this.notificationService = notificationService;
    }

    public override void ProcessRequest(DeleteServiceUseCaseRequest request)
    {
        request.appointments = this.repository.Find(e => e.ServiceId == request.ServiceId).ToList();
        sucessor?.ProcessRequest(request);
    }
}

