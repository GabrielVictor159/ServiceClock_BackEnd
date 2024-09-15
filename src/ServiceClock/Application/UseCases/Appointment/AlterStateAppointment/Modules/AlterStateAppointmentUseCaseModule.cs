using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Application.UseCases.Appointment.AlterStateAppointment.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Appointment.RequestAppointment;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Domain.Modules;
using ServiceClock_BackEnd.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Application.UseCases.Appointment.AlterStateAppointment.Modules;
public class AlterStateAppointmentUseCaseModule : Module
{
    public override void Configure(IServiceCollection services)
    {
        services.AddTransient<IAlterStateAppointmentUseCase, AlterStateAppointmentUseCase>();

        services.AddSingleton<ValidProcessAlterStateAppointmentHandler>();
        services.AddSingleton<AlterStateAppointmentHandler>();
    }
}
