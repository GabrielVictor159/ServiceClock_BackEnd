
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd;
using ServiceClock_BackEnd.Application.Modules;
using ServiceClock_BackEnd.Infraestructure.Modules;
using ServiceClock_BackEnd.Modules;

namespace ServiceClock_BackEnd;

public static class ServiceCollectionExtensions
{
    public static void ConfigureServicesModules(this IServiceCollection services)
    {
        services.AddModule(new ApiModule());
        services.AddModule(new ApplicationModule());
        services.AddModule(new InfraestructureModule());
    }


    private static void AddModule(this IServiceCollection services, Domain.Modules.Module module)
    {
        module.Configure(services);
    }
}

