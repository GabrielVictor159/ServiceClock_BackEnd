
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Application.UseCases.Services.DeleteService.Handlers;

public class DeleteServiceHandler : Handler<DeleteServiceUseCaseRequest>
{
    private readonly IRepository<Domain.Models.Appointment> repositoryAppointment;
    private readonly IRepository<Service> repositoryService;
    private readonly INotificationService notificationService;

    public DeleteServiceHandler
        (IRepository<Domain.Models.Appointment> repositoryAppointment, 
        IRepository<Service> repositoryService, 
        INotificationService notificationService, 
        ILogService logService)
        : base(logService)
    {
        this.repositoryAppointment = repositoryAppointment;
        this.repositoryService = repositoryService;
        this.notificationService = notificationService;
    }

    public override void ProcessRequest(DeleteServiceUseCaseRequest request)
    {
        this.repositoryAppointment.DeleteRange(request.appointments);
        this.repositoryService.Delete(request.Service!);
        request.IsDeleted = true;
        sucessor?.ProcessRequest(request);
    }
}

