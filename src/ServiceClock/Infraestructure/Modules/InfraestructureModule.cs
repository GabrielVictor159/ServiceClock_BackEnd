
using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Domain.Modules;
using ServiceClock_BackEnd.Infraestructure.Services;

namespace ServiceClock_BackEnd.Infraestructure.Modules;

public class InfraestructureModule : Module
{
    public override void Configure(IServiceCollection services)
    {
        services.AddSingleton<INotificationService, NotificationService>();
    }
}

