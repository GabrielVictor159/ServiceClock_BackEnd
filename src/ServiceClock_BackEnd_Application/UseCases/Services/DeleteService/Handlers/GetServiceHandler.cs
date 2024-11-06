
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Application.UseCases.Services.DeleteService.Handlers;

public class GetServiceHandler : Handler<DeleteServiceUseCaseRequest>
{
    private readonly IRepository<Service> repository;
    private readonly INotificationService notificationService;
    public GetServiceHandler
        (ILogService logService,
        IRepository<Service> repository,
        INotificationService notificationService) 
        : base(logService)
    {
        this.repository = repository;
        this.notificationService = notificationService;
    }

    public override void ProcessRequest(DeleteServiceUseCaseRequest request)
    {
        request.Service = repository.FindSingle(e => e.Id == request.ServiceId && e.CompanyId==request.CompanyId);
        if(request.Service==null)
        {
            this.notificationService.AddNotification("Service not found", "Serviço não encontrado");
            return;
        }
        sucessor?.ProcessRequest(request);
    }
}

