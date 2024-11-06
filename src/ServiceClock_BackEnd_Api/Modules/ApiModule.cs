
using Autofac;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using ServiceClock_BackEnd.Infraestructure;
using ServiceClock_BackEnd_Api.Factory;
using ServiceClock_BackEnd_Api.Factory.Handlers;
using System.Reflection;

namespace ServiceClock_BackEnd.Modules
{
    public class ApiModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
           .Where(t => typeof(WebSocketHandler<>).IsAssignableFrom(t) && t != typeof(WebSocketHandler<>))
           .AsSelf()
           .InstancePerLifetimeScope();

            builder.RegisterType<WebSocketFactory>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(ApiException).Assembly)
                     .AsImplementedInterfaces().AsSelf().InstancePerLifetimeScope();

            this.Mapper(builder);
        }

        private void Mapper(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ApiException).Assembly)
              .Where(t => (t.Namespace ?? string.Empty).Contains("Mapper") && typeof(AutoMapper.Profile).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic)
              .As<AutoMapper.Profile>();
            builder.Register(c => new MapperConfiguration(cfg =>
            {
                foreach (var profile in c.Resolve<IEnumerable<AutoMapper.Profile>>())
                {
                    cfg.AddProfile(profile);
                }
                cfg.AddExpressionMapping();
            })).AsSelf().SingleInstance();
            builder.Register(c => c.Resolve<MapperConfiguration>()
                .CreateMapper(c.Resolve))
            .As<IMapper>()
            .InstancePerLifetimeScope();
        }
    }
}
