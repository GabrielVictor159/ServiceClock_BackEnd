
using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Application.UseCases.Services.DeleteService.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Services.DeleteService;
using ServiceClock_BackEnd.Domain.Modules;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Domain.Validations;

namespace ServiceClock_BackEnd.Application.UseCases.Services.EditService.Modules;

public class EditServiceUseCaseModule : Module
{
    public override void Configure(IServiceCollection services)
    {
        services.AddTransient<IEditServiceUseCase, EditServiceUseCase>();

        services.AddSingleton<ValidDomainHandler<Service, ServiceValidator, EditServiceUseCaseRequest>>();
        services.AddSingleton<SaveChangesRepositoryHandler<Service, EditServiceUseCaseRequest>>();
    }
}

