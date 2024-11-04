
namespace ServiceClock_BackEnd.Application.Boundaries.Client;

public class DeleteClientBoundarie
{ 
    public List<Domain.Models.Appointment> Appointments { get; set; } = new();
    public List<Domain.Models.Message> Messages { get; set; } = new();

    public int ClientDeleteRows { get; set; } = 0;
    public int AppointmentsDeleteRows { get; set; } = 0;
    public int MessagesDeleteRows { get; set; } = 0;
}

