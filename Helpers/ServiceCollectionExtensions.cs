
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Api.Modules;
using ServiceClock_BackEnd.Infraestructure.Modules;

namespace ServiceClock_BackEnd.Helpers;

public static class ServiceCollectionExtensions
{
    public static void ConfigureServicesModules(this IServiceCollection services)
    {
        services.AddModule(new ApiModule());
        services.AddModule(new ApplicationModule());
        //services.AddModule(new InfraestructureModule());
    }


    private static void AddModule(this IServiceCollection services, Domain.Modules.Module module)
    {
        module.Configure(services);
    }
}

