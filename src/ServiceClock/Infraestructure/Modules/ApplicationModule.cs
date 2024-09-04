
using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Application.UseCases.CreateCompany.Modules;
using ServiceClock_BackEnd.Application.UseCases.PatchCompany.Modules;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Domain.Modules;
using ServiceClock_BackEnd.Infraestructure.Data.Repositories;
using ServiceClock_BackEnd.Infraestructure.Services;

namespace ServiceClock_BackEnd.Infraestructure.Modules;

public class ApplicationModule : Module
{
    public override void Configure(IServiceCollection services)
    {
        AddServices(services);
        AddRepositories(services);
        AddUseCases(services);
    }
    private void AddUseCases(IServiceCollection services)
    {
        new CreateCompanyModule().Configure(services);
        new PatchCompanyModule().Configure(services);
    }
    private void AddServices(IServiceCollection services)
    {
        services.AddSingleton<ITokenService, TokenService>();
    }
    private void AddRepositories(IServiceCollection services)
    {
        services.AddSingleton<IRepository<Company>, Repository<Company>>();
        services.AddSingleton<IRepository<Client>, Repository<Client>>();
        services.AddSingleton<IRepository<Log>, Repository<Log>>();
    }
}

