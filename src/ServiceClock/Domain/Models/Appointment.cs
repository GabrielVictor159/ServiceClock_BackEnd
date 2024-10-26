
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Validations;

namespace ServiceClock_BackEnd.Domain.Models;

public class Appointment : Entity<Appointment,AppointmentValidator>
{
    public Appointment() 
        : base(new())
    {
    }

    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public Guid ClientId { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = "";
    public AppointmentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

}

