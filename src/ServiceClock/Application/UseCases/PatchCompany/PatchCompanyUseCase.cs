
using ServiceClock_BackEnd.Api.UseCases.Company.PatchCompany;
using ServiceClock_BackEnd.Application.Boundaries;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Infraestructure.Data.Repositories;

namespace ServiceClock_BackEnd.Application.UseCases.PatchCompany;

public class PatchCompanyUseCase : IPatchCompanyUseCase
{
    private readonly IRepository<Company> repository;
    private readonly INotificationService notificationService;
    private readonly ILogService logService;
    private readonly PatchCompanyPresenter outputPort;

    public PatchCompanyUseCase
        (IRepository<Company> repository, 
        INotificationService notificationService,
        PatchCompanyPresenter outputPort,
        ILogService logService)
    {
        this.repository = repository;
        this.notificationService = notificationService;
        this.logService = logService;
        this.outputPort = outputPort;
    }

    public void Execute(PatchCompanyUseCaseRequest request)
    {
        try
        {
            var company = repository.GetById(request.Company?.Id.ToString() ?? "");
            if (company == null)
            {
                this.notificationService.AddNotification("Company not found", "Não foi encontrado nenhuma empresa com esse Id");
                return;
            }
            company.Name = request.Company!.Name != "" ? request.Company.Name : company.Name;
            company.Password = request.Company!.Password != "" ? request.Company.Password : company.Password;
            company.RegistrationNumber = request.Company!.RegistrationNumber != "" ? request.Company.RegistrationNumber : company.RegistrationNumber;
            company.Address = request.Company!.Address != "" ? request.Company.Address : company.Address;
            company.City = request.Company!.City != "" ? request.Company.City : company.City;
            company.State = request.Company!.State != "" ? request.Company.State : company.State;
            company.Country = request.Company!.Country != "" ? request.Company.Country : company.Country;
            company.PostalCode = request.Company!.PostalCode != "" ? request.Company.PostalCode : company.PostalCode;
            company.PhoneNumber = request.Company!.PhoneNumber != "" ? request.Company.PhoneNumber : company.PhoneNumber;

            if(!company.IsValid)
            {
                this.notificationService.AddNotifications(company.ValidationResult);
                return;
            }

            repository.Save();

            outputPort.Standard(new() { Company = company });
        }
        catch (Exception ex)
        {
            this.logService.logs.Add(new(LogType.ERROR, "CreateCompanyUseCase", $"Occurring an error: {ex.Message ?? ex.InnerException?.Message}, stacktrace: {ex.StackTrace}"));
            outputPort.Error(ex.Message!);
        }
    }
}

