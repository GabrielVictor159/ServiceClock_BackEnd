using ServiceClock_BackEnd.Application.Boundaries.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Api.UseCases.Appointment.RequestAppointment;
public class RequestAppointmentResponse
{
    public RequestAppointmentResponse(RequestAppointmentBoundarie boundarie)
    {
        if(boundarie.Appointment!=null)
        {
            this.AppointmentId = boundarie.Appointment.Id;
            this.Success = true;
        }
    }
    public Guid AppointmentId { get; set; }
    public bool Success { get; set; } = false;
}
