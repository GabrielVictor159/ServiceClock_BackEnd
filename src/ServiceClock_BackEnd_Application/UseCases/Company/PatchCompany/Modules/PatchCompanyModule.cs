
using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany;
using ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany.Handlers;
using ServiceClock_BackEnd.Domain.Modules;
using ServiceClock_BackEnd.Domain.Validations;

namespace ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany.Modules;

public class PatchCompanyModule : Module
{
    public override void Configure(IServiceCollection services)
    {
        services.AddTransient<IPatchCompanyUseCase, PatchCompanyUseCase>();

        services.AddSingleton<SaveChangesRepositoryHandler<Domain.Models.Company, PatchCompanyUseCaseRequest>>();
        services.AddSingleton<ValidDomainHandler<Domain.Models.Company, CompanyValidator, PatchCompanyUseCaseRequest>>();
        services.AddSingleton<VerifyDisponibilityCompanyHandler<PatchCompanyUseCaseRequest>>();
        services.AddSingleton<SearchCompanyForUpdateHandler>();
        services.AddSingleton<SaveImageHandler>();
    }
}

