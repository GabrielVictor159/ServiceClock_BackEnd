
using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Services.CreateService;
using ServiceClock_BackEnd.Application.UseCases.Services.DeleteService.Handlers;
using ServiceClock_BackEnd.Domain.Modules;
using ServiceClock_BackEnd.Domain.Validations;

namespace ServiceClock_BackEnd.Application.UseCases.Services.DeleteService.Modules;

public class DeleteServiceUseCaseModule : Module
{
    public override void Configure(IServiceCollection services)
    {
        services.AddTransient<IDeleteServiceUseCase, DeleteServiceUseCase>();

        services.AddSingleton<DeleteServiceHandler>();
        services.AddSingleton<GetAppointmentsHandler>();
        services.AddSingleton<GetServiceHandler>();
    }
}

