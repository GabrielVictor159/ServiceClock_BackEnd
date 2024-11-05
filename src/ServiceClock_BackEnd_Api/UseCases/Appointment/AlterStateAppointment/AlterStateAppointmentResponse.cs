using ServiceClock_BackEnd.Application.Boundaries.Appointment;
using ServiceClock_BackEnd.UseCases;

namespace ServiceClock_BackEnd.UseCases.Appointment.AlterStateAppointment;
public class AlterStateAppointmentResponse : ResponseCore
{
    public AlterStateAppointmentResponse(AlterStateAppointmentBoundarie boundarie)
        : base("Appointment")
    {
        if(boundarie.Appointment!=null && boundarie.Appointment.Status==boundarie.Status)
        {
            this.Success = true;
        }
    }
    public bool Success { get; set; } = false;
}
