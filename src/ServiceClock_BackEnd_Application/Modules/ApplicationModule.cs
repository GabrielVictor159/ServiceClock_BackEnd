
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceClock_BackEnd.Application.Modules;

public class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(ApplicationException).Assembly)
                 .AsImplementedInterfaces().AsSelf().InstancePerLifetimeScope();
    }

}

