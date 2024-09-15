using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Application.UseCases.Client.CreateClient.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Client.CreateClient;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Domain.Modules;
using ServiceClock_BackEnd.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceClock_BackEnd.Application.UseCases.Appointment.RequestAppointment.Handlers;

namespace ServiceClock_BackEnd.Application.UseCases.Appointment.RequestAppointment.Modules;
public class RequestAppointmentUseCaseModule : Module
{
    public override void Configure(IServiceCollection services)
    {
        services.AddTransient<IRequestAppointmentUseCase, RequestAppointmentUseCase>();

        services.AddSingleton<ValidDomainHandler<Domain.Models.Appointment, AppointmentValidator, RequestAppointmentUseCaseRequest>>();
        services.AddSingleton<SaveDomainDbHandler<Domain.Models.Appointment, RequestAppointmentUseCaseRequest>>();
        services.AddSingleton<SearchEntitiesDbAppointmentHandler>();
    }
}
