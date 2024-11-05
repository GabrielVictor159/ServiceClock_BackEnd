using Newtonsoft.Json;

namespace ServiceClock_BackEnd.UseCases.Appointment.AlterStateAppointment;
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
