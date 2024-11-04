
using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Application.UseCases.Appointment.AlterStateAppointment.Modules;
using ServiceClock_BackEnd.Application.UseCases.Appointment.RequestAppointment.Modules;
using ServiceClock_BackEnd.Application.UseCases.Client.CreateClient.Modules;
using ServiceClock_BackEnd.Application.UseCases.Client.DeleteClient.Modules;
using ServiceClock_BackEnd.Application.UseCases.Client.PatchClient.Modules;
using ServiceClock_BackEnd.Application.UseCases.Company.CreateCompany.Modules;
using ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany.Modules;
using ServiceClock_BackEnd.Application.UseCases.Messages.CreateMessage.Modules;
using ServiceClock_BackEnd.Application.UseCases.Services.CreateService.Modules;
using ServiceClock_BackEnd.Application.UseCases.Services.DeleteService.Modules;
using ServiceClock_BackEnd.Application.UseCases.Services.EditService.Modules;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Domain.Modules;

namespace ServiceClock_BackEnd.Application.Modules;

public class ApplicationModule : Module
{
    public override void Configure(IServiceCollection services)
    {
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
        new DeleteClientModule().Configure(services);
    }
}

