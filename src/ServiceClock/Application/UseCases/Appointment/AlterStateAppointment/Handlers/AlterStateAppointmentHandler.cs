using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Application.UseCases.Appointment.AlterStateAppointment.Handlers;
public class AlterStateAppointmentHandler : Handler<AlterStateAppointmentUseCaseRequest>
{
    private readonly IRepository<Domain.Models.Appointment> repository;
    public AlterStateAppointmentHandler
        (ILogService logService,
        IRepository<Domain.Models.Appointment> repository) : base(logService)
    {
        this.repository = repository;
    }

    public override void ProcessRequest(AlterStateAppointmentUseCaseRequest request)
    {
        if(request.Appointment!=null)
        {
            request.Appointment.Status = request.Status;
            repository.Update(request.Appointment);
        }
       sucessor?.ProcessRequest(request);
    }
}
