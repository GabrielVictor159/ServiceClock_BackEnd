
using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Domain.Modules;
using ServiceClock_BackEnd.Infraestructure.Data.Repositories;
using ServiceClock_BackEnd.Infraestructure.Services;

namespace ServiceClock_BackEnd.Infraestructure.Modules;

public class InfraestructureModule : Module
{
    public override void Configure(IServiceCollection services)
    {
        services.AddSingleton<INotificationService, NotificationService>();
        services.AddSingleton<ILogService, LogService>();
        services.AddSingleton<IBlobService, BlobService>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddLogging();
        AddRepositories(services);
    }

    private void AddRepositories(IServiceCollection services)
    {
        services.AddSingleton<IRepository<Company>, Repository<Company>>();
        services.AddSingleton<IRepository<Client>, Repository<Client>>();
        services.AddSingleton<IRepository<Service>, Repository<Service>>();
        services.AddSingleton<IRepository<Appointment>, Repository<Appointment>>();
        services.AddSingleton<IRepository<Message>, Repository<Message>>();
        services.AddSingleton<IRepository<Log>, Repository<Log>>();
    }
}

