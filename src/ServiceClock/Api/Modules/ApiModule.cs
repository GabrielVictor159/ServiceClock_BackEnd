
using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Api.Filters;
using ServiceClock_BackEnd.Api.Mapper;
using ServiceClock_BackEnd.Api.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.Api.Validator.Http;
using ServiceClock_BackEnd.Domain.Modules;

namespace ServiceClock_BackEnd.Api.Modules;

public class ApiModule : Module
{
    public override void Configure(IServiceCollection services)
    {
        services.AddSingleton(new HttpRequestValidator());
        services.AddScoped<NotificationMiddleware>();
        services.AddAutoMapper(typeof(MapperProfile).Assembly);

        services.AddSingleton<CreateCompanyPresenter>();
    }
}

