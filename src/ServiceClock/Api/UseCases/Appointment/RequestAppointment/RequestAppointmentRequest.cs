using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Api.UseCases.Appointment.RequestAppointment;
public class RequestAppointmentRequest
{
    [JsonProperty("Date")]
    public DateTime Date { get; set; }
    [JsonProperty("Description")]
    public string Description { get; set; } = "";
    [JsonIgnore]
    public Guid clientId { get; set; }
}
