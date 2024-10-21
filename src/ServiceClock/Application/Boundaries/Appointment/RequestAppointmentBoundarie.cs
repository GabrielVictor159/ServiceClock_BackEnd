using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Application.Boundaries.Appointment;
public class RequestAppointmentBoundarie
{
    public Domain.Models.Appointment? Appointment { get; set; }
    public Domain.Models.Client? Client { get; set; }
}
