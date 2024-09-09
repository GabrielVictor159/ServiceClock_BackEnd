
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;

namespace ServiceClock_BackEnd.Application.UseCases.Common.Handlers;

public class VerifyDisponibilityServiceHandler<Request> : Handler<Request>
{
    private readonly IRepository<Domain.Models.Service> repository;
    private readonly INotificationService notificationService;
    public VerifyDisponibilityServiceHandler
        (ILogService logService,
        IRepository<Domain.Models.Service> repository,
        INotificationService notificationService) 
        : base(logService)
    {
        this.repository = repository;
        this.notificationService = notificationService;
    }

    public override void ProcessRequest(Request request)
    {
        var domainObject = typeof(Request)
                .GetProperties()
                .FirstOrDefault(prop => prop.PropertyType == typeof(Domain.Models.Service))?
                .GetValue(request) as Domain.Models.Service;

        if (domainObject == null)
        {
            throw new ApplicationException($"Could not find any object with the type Service in the request");
        }

        if (repository.FindSingle(e => (e.Name == domainObject.Name) && e.CompanyId == domainObject.CompanyId) != null)
        {
            this.notificationService.AddNotification("Service already exists", "Já existe um serviço registrado com o mesmo nome");
            return;
        }
        sucessor?.ProcessRequest(request);
    }
}

