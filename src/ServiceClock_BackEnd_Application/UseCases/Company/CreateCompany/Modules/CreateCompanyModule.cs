
using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Domain.Modules;
using ServiceClock_BackEnd.Domain.Validations;

namespace ServiceClock_BackEnd.Application.UseCases.Company.CreateCompany.Modules;

public class CreateCompanyModule : Module
{
    public override void Configure(IServiceCollection services)
    {
        services.AddTransient<ICreateCompanyUseCase, CreateCompanyUseCase>();

        services.AddSingleton<ValidDomainHandler<Domain.Models.Company, CompanyValidator, CreateCompanyUseCaseRequest>>();
        services.AddSingleton<VerifyDisponibilityCompanyHandler<CreateCompanyUseCaseRequest>>();
        services.AddSingleton<SaveDomainDbHandler<Domain.Models.Company, CreateCompanyUseCaseRequest>>();

    }
}

