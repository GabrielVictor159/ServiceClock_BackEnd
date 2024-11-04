
using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.Domain.Modules;
using ServiceClock_BackEnd.Domain.Validations;

namespace ServiceClock_BackEnd.Application.UseCases.Services.CreateService.Modules;

public class CreateServiceModule : Module
{
    public override void Configure(IServiceCollection services)
    {
        services.AddTransient<ICreateServiceUseCase, CreateServiceUseCase>();

        services.AddSingleton<ValidDomainHandler<Domain.Models.Service, ServiceValidator, CreateServiceUseCaseRequest>>();
        services.AddSingleton<VerifyDisponibilityServiceHandler<CreateServiceUseCaseRequest>>();
        services.AddSingleton<SaveDomainDbHandler<Domain.Models.Service, CreateServiceUseCaseRequest>>();
    }
}

