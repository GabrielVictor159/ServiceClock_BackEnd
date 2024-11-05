using Autofac;
using ServiceClock_BackEnd.Application.Modules;
using ServiceClock_BackEnd.Infraestructure.Modules;
using ServiceClock_BackEnd.Modules;

namespace ServiceClock_BackEnd_Api.Modules.DependencyInjection;

public static class AutofacExtensions
{
    public static ContainerBuilder AddAutofacRegistration(this ContainerBuilder builder)
    {
        builder.RegisterModule<ApiModule>();
        builder.RegisterModule<InfraestructureModule>();
        builder.RegisterModule<ApplicationModule>();
        return builder;
    }
}
