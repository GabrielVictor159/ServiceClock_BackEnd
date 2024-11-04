using Newtonsoft.Json;
using ServiceClock_BackEnd.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceClock_BackEnd.UseCases.Appointment.ListAppointment;
public class ListAppointmentRequest
{
    [JsonProperty("Id")]
    public Guid Id { get; set; }
    [JsonProperty("ServiceId")]
    public Guid ServiceId { get; set; }
    [JsonProperty("ClientId")]
    public Guid ClientId { get; set; }
    [JsonProperty("MinDate")]
    public DateTime MinDate { get; set; } = DateTime.MinValue;
    [JsonProperty("MaxDate")]
    public DateTime MaxDate { get; set; } = DateTime.MaxValue;
    [JsonProperty("Description")]
    public string Description { get; set; } = "";
    [JsonProperty("Status")]
    public AppointmentStatus? Status { get; set; }
    [JsonProperty("CreatedAt")]
    public DateTime CreatedAt { get; set; } = DateTime.MinValue;
    [JsonProperty("IndexPage")]
    [Range(1, int.MaxValue, ErrorMessage = "IndexPage must be greater than or equal to 1.")]
    public int IndexPage { get; set; } = 1;
    [JsonProperty("PageSize")]
    public PageSize PageSize { get; set; } = PageSize.Minimal;
}
