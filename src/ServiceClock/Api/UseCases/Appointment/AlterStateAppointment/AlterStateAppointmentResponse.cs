using ServiceClock_BackEnd.Application.Boundaries.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Api.UseCases.Appointment.AlterStateAppointment;
public class AlterStateAppointmentResponse
{
    public AlterStateAppointmentResponse(AlterStateAppointmentBoundarie boundarie)
    {
        if(boundarie.Appointment!=null && boundarie.Appointment.Status==boundarie.Status)
        {
            this.Success = true;
        }
    }
    public bool Success { get; set; } = false;
}
