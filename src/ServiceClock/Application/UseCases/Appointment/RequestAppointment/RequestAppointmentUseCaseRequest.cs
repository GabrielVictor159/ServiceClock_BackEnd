using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Application.UseCases.Appointment.RequestAppointment;
public class RequestAppointmentUseCaseRequest
{
    public RequestAppointmentUseCaseRequest(Guid serviceId, Guid clientId, DateTime date, string description)
    {
        this.Appointment = new Domain.Models.Appointment() 
        {
            Id = Guid.NewGuid(),
            ServiceId = serviceId,
            ClientId = clientId,
            Date = date,
            Description = description,
            CreatedAt = DateTime.Now,
            Status = Domain.Enums.AppointmentStatus.PendingApproval,
        };
    }

    public Domain.Models.Appointment Appointment { get; set; }
    public Domain.Models.Client? Client { get; set; }
}
