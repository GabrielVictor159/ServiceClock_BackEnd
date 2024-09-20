
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Infraestructure.Services;

namespace ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany.Handlers;

public class SearchCompanyForUpdateHandler : Handler<PatchCompanyUseCaseRequest>
{
    private readonly IRepository<Domain.Models.Company> repository;
    private readonly INotificationService notificationService;
    public SearchCompanyForUpdateHandler
        (IRepository<Domain.Models.Company> repository,
        ILogService logService,
        INotificationService notificationService) 
        : base(logService)
    {
        this.repository = repository;
        this.notificationService = notificationService;
    }

    public override void ProcessRequest(PatchCompanyUseCaseRequest request)
    {
        var company = repository.GetById(request.Company!.Id);
        if (company == null)
        {
            notificationService.AddNotification("Company not found", "Não foi encontrado nenhuma empresa com esse Id");
            return;
        }
        company.Name = request.Company!.Name != "" ? request.Company.Name : company.Name;
        company.Password = request.Company!.Password != "" ? request.Company.Password : company.Password;
        company.RegistrationNumber = request.Company!.RegistrationNumber != "" ? request.Company.RegistrationNumber : company.RegistrationNumber;
        company.Address = request.Company!.Address != "" ? request.Company.Address : company.Address;
        company.City = request.Company!.City != "" ? request.Company.City : company.City;
        company.State = request.Company!.State != "" ? request.Company.State : company.State;
        company.PostalCode = request.Company!.PostalCode != "" ? request.Company.PostalCode : company.PostalCode;
        company.PhoneNumber = request.Company!.PhoneNumber != "" ? request.Company.PhoneNumber : company.PhoneNumber;

        request.Company = company;

        sucessor?.ProcessRequest(request);
    }
}

