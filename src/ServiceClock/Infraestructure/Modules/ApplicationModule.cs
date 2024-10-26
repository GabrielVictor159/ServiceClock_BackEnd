
using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Application.UseCases.Appointment.AlterStateAppointment.Modules;
using ServiceClock_BackEnd.Application.UseCases.Appointment.RequestAppointment.Modules;
using ServiceClock_BackEnd.Application.UseCases.Client.CreateClient.Modules;
using ServiceClock_BackEnd.Application.UseCases.Client.PatchClient.Modules;
using ServiceClock_BackEnd.Application.UseCases.Company.CreateCompany.Modules;
using ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany.Modules;
using ServiceClock_BackEnd.Application.UseCases.Messages.CreateMessage.Modules;
using ServiceClock_BackEnd.Application.UseCases.Services.CreateService.Modules;
using ServiceClock_BackEnd.Application.UseCases.Services.DeleteService.Modules;
using ServiceClock_BackEnd.Application.UseCases.Services.EditService.Modules;
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
        new CreateClientModule().Configure(services);
        new PatchClientModule().Configure(services);
        new CreateServiceModule().Configure(services);
        new DeleteServiceUseCaseModule().Configure(services);
        new RequestAppointmentUseCaseModule().Configure(services);
        new AlterStateAppointmentUseCaseModule().Configure(services);
        new CreateMessageModule().Configure(services);
        new EditServiceUseCaseModule().Configure(services);
    }
    private void AddServices(IServiceCollection services)
    {
        services.AddSingleton<ITokenService, TokenService>();
    }
    private void AddRepositories(IServiceCollection services)
    {
        services.AddSingleton<IRepository<Company>, Repository<Company>>();
        services.AddSingleton<IRepository<Client>, Repository<Client>>();
        services.AddSingleton<IRepository<Service>, Repository<Service>>();
        services.AddSingleton<IRepository<Appointment>, Repository<Appointment>>();
        services.AddSingleton<IRepository<Message>, Repository<Message>>();
        services.AddSingleton<IRepository<Log>, Repository<Log>>();
    }
}

