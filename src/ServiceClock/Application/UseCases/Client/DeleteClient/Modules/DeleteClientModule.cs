
using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Application.UseCases.Client.DeleteClient.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Client.PatchClient;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Domain.Modules;

namespace ServiceClock_BackEnd.Application.UseCases.Client.DeleteClient.Modules;

public class DeleteClientModule : Module
{
    public override void Configure(IServiceCollection services)
    {
        services.AddTransient<IDeleteClientUseCase, DeleteClientUseCase>();

        services.AddSingleton<GetAppointmentsAndMessagesHandler>();
        services.AddSingleton<DeleteMessagesHandler>();
        services.AddSingleton<DeleteClientHandler>();
        services.AddSingleton<DeleteAppointmentsHandler>();
    }
}

