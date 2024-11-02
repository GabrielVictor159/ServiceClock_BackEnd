using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
public class VerifyDisponibilityClientHandler<Request> : Handler<Request>
{
    private readonly IRepository<Domain.Models.Client> companyRepository;
    private readonly INotificationService notificationService;

    public VerifyDisponibilityClientHandler
        (IRepository<Domain.Models.Client> companyRepository,
        INotificationService notificationService,
        ILogService logService)
        : base(logService)
    {
        this.companyRepository = companyRepository;
        this.notificationService = notificationService;
    }

    public override void ProcessRequest(Request request)
    {
        var domainObject = typeof(Request)
                .GetProperties()
                .FirstOrDefault(prop => prop.PropertyType == typeof(Domain.Models.Client))?
                .GetValue(request) as Domain.Models.Client;

        if (domainObject == null)
        {
            throw new ApplicationException($"Could not find any object with the type Client in the request");
        }

        if (companyRepository.FindSingle(e => (e.Name == domainObject.Name || e.Email == domainObject.Email) && e.Id != domainObject.Id && e.Active==true) != null)
        {
            this.notificationService.AddNotification("Client already exists", "Já existe um cliente com o mesmo nome ou email");
            return;
        }

        this.sucessor?.ProcessRequest(request);
    }
}