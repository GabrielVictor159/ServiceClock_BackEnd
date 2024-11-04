
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServiceClock_BackEnd.Filters;
using ServiceClock_BackEnd.Mapper;

using ServiceClock_BackEnd.Domain.Modules;
using System.Configuration;
using System.Text;

namespace ServiceClock_BackEnd.Modules
{
    public class ApiModule : Module
    {
        public override void Configure(IServiceCollection services)
        {
            services.AddSingleton(new HttpRequestValidator());
            services.AddScoped<NotificationMiddleware>();
            services.AddAutoMapper(typeof(MapperProfile).Assembly);
            services.AddSingleton<CreateCompanyPresenter>();
            services.AddSingleton<PatchCompanyPresenter>();
            services.AddSingleton<CreateClientPresenter>();
            services.AddSingleton<PatchClientPresenter>();
            services.AddSingleton<CreateServicePresenter>();
            services.AddSingleton<DeleteServicePresenter>();
            services.AddSingleton<RequestAppointmentPresenter>();
            services.AddSingleton<AlterStateAppointmentPresenter>();
            services.AddSingleton<CreateMessagePresenter>();
            services.AddSingleton<EditServicePresenter>();
            services.AddSingleton<DeleteClientPresenter>();

        }
    }
}
