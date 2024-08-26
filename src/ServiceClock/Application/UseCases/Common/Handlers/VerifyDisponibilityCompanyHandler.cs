
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Application.UseCases.Common.Handlers;

public class VerifyDisponibilityCompanyHandler<Request> : Handler<Request>
{
    private readonly IRepository<Company> companyRepository;
    private readonly INotificationService notificationService;

    public VerifyDisponibilityCompanyHandler
        (IRepository<Company> companyRepository, 
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
                .FirstOrDefault(prop => prop.PropertyType == typeof(Company))?
                .GetValue(request) as Company;

        if (domainObject == null)
        {
            throw new ApplicationException($"Could not find any object with the type Comapny in the request");
        }

        if(companyRepository.FindSingle(e=>e.Name==domainObject.Name || e.Email == domainObject.Email || e.RegistrationNumber == domainObject.RegistrationNumber) !=null)
        {
            this.notificationService.AddNotification("Company already exists", "Já existe uma empresa com as mesmas propriedades");
            return;
        }

        this.sucessor?.ProcessRequest(request);
    }
}

