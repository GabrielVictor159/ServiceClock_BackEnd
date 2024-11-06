
namespace ServiceClock_BackEnd.Application.UseCases.Client.DeleteClient;

public class DeleteClientUseCaseRequest
{
    public DeleteClientUseCaseRequest(Domain.Models.Client client)
    {
        Client = client;
    }

    public Domain.Models.Client Client { get; set; }

    public List<Domain.Models.Appointment> Appointments { get; set; }  = new();
    public List<Domain.Models.Message> Messages { get; set; } = new();

    public int ClientDeleteRows { get; set; } = 0;
    public int AppointmentsDeleteRows { get; set; } = 0;
    public int MessagesDeleteRows { get; set; } = 0;
}

