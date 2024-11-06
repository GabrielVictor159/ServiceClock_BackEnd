
using ServiceClock_BackEnd.Application.Boundaries.Client;

namespace ServiceClock_BackEnd.UseCases.Client.DeleteClient;

public class DeleteClientResponse
{
    public DeleteClientResponse(DeleteClientBoundarie boundarie)
    {
        this.Success = true;
        if(boundarie.ClientDeleteRows <1)
        {
            this.Success=false;
            this.Error = "Não foi possivel deletar o Cliente";
        }
        if(boundarie.MessagesDeleteRows < boundarie.Messages.Count)
        {
            this.Success=false;
            this.Error = "Não foi possivel deletar todas as mensagens do Cliente";
        }
        if(boundarie.AppointmentsDeleteRows < boundarie.Appointments.Count)
        {
            this.Success = false;
            this.Error = "Não foi possivel deletar todos os apontamentos do cliente";
        }
    }
    public bool Success { get; set; } = false;
    public string Error { get; set; } = "";
}

