using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Api.UseCases.Appointment.AlterStateAppointment;
public class AlterStateAppointmentRequest
{
    [JsonProperty("AppointmentId")]
    public Guid AppointmentId { get; set; }
    [JsonProperty("Status")]
    public Domain.Enums.AppointmentStatus Status { get; set; }
    [JsonIgnore]
    public string UserType { get; set; } = "";
    [JsonIgnore]
    public Guid UserId { get; set; }
}
