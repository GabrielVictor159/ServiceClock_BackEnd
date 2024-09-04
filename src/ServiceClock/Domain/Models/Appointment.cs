
using ServiceClock_BackEnd.Domain.Enums;

namespace ServiceClock_BackEnd.Domain.Models;

public class Appointment
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public Guid ClientId { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = "";
    public AppointmentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

}

