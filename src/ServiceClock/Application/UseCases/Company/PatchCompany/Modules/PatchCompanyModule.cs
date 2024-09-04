
using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany;
using ServiceClock_BackEnd.Application.UseCases.CreateCompany;
using ServiceClock_BackEnd.Domain.Modules;

namespace ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany.Modules;

public class PatchCompanyModule : Module
{
    public override void Configure(IServiceCollection services)
    {
        services.AddTransient<IPatchCompanyUseCase, PatchCompanyUseCase>();
    }
}

