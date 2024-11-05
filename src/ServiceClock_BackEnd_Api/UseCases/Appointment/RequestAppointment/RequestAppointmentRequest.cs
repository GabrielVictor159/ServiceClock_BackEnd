using Newtonsoft.Json;

namespace ServiceClock_BackEnd.UseCases.Appointment.RequestAppointment;
public class RequestAppointmentRequest
{
    [JsonProperty("Date")]
    public DateTime Date { get; set; }
    [JsonProperty("Description")]
    public string Description { get; set; } = "";
    [JsonProperty("ServiceId")]
    public Guid ServiceId { get; set; }
    [JsonIgnore]
    public Guid clientId { get; set; }
}
