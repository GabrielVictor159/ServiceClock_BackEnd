
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Application.UseCases.Common.Handlers;

public class VerifyDisponibilityCompanyHandler<Request> : Handler<Request>
{
    private readonly IRepository<Domain.Models.Company> companyRepository;
    private readonly INotificationService notificationService;

    public VerifyDisponibilityCompanyHandler
        (IRepository<Domain.Models.Company> companyRepository, 
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
                .FirstOrDefault(prop => prop.PropertyType == typeof(Domain.Models.Company))?
                .GetValue(request) as Domain.Models.Company;

        if (domainObject == null)
        {
            throw new ApplicationException($"Could not find any object with the type Company in the request");
        }

        if(companyRepository.FindSingle(e=>(e.Name==domainObject.Name || e.Email == domainObject.Email || e.RegistrationNumber == domainObject.RegistrationNumber) && e.Id != domainObject.Id) !=null)
        {
            this.notificationService.AddNotification("Company already exists", "Já existe uma empresa com o mesmo nome, email ou numero de registro");
            return;
        }

        this.sucessor?.ProcessRequest(request);
    }
}

