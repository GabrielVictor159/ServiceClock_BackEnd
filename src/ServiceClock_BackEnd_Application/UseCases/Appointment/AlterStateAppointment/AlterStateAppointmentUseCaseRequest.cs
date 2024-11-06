using Newtonsoft.Json;
using ServiceClock_BackEnd.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Application.UseCases.Appointment.AlterStateAppointment;
public class AlterStateAppointmentUseCaseRequest
{
    public AlterStateAppointmentUseCaseRequest
        (Guid appointmentId, 
        AppointmentStatus status, 
        string userType, 
        Guid userId)
    {
        AppointmentId = appointmentId;
        Status = status;
        UserType = userType;
        UserId = userId;
    }

    public Guid AppointmentId { get; set; }
    public Domain.Enums.AppointmentStatus Status { get; set; }
    public string UserType { get; set; } = "";
    public Guid UserId { get; set; }

    public Domain.Models.Appointment? Appointment { get; set; } 
}
