
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;

namespace ServiceClock_BackEnd.Application.Modules;

public class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(ApplicationException).Assembly)
                 .AsImplementedInterfaces().AsSelf().InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(ValidDomainHandler<,,>))
               .AsSelf()
               .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(SaveDomainDbHandler<,>))
               .AsSelf()
               .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(SaveChangesRepositoryHandler<,>))
               .AsSelf()
               .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(VerifyDisponibilityCompanyHandler<>))
               .AsSelf()
               .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(VerifyDisponibilityClientHandler<>))
               .AsSelf()
               .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(VerifyDisponibilityServiceHandler<>))
               .AsSelf()
               .InstancePerLifetimeScope();
    }

}

