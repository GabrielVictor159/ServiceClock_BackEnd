
using Autofac;
using ServiceClock_BackEnd.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ServiceClock_BackEnd.Infraestructure.Modules;

public class InfraestructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(InfraestructureException).Assembly)
        .AsImplementedInterfaces().AsSelf().InstancePerLifetimeScope();

        DataAccess(builder);
        base.Load(builder);
    }

    private void DataAccess(ContainerBuilder builder)
    {
        var connection = Environment.GetEnvironmentVariable("DBCONN");

        builder.RegisterAssemblyTypes(typeof(InfraestructureException).Assembly)
            .Where(t => (t.Namespace ?? string.Empty).Contains("Database"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        if (!string.IsNullOrEmpty(connection))
        {
            try
            {
                using var context = new Context();
                context.Database.Migrate();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}

