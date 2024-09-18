namespace ServiceClock_BackEnd.Api.UseCases.Client.PatchClient;

public class PatchClientResponse : ResponseCore
{
    public PatchClientResponse(Application.Boundaries.Client.PatchClientBoundarie? boundarie)
        : base("Client")
    {
        if(boundarie !=null && boundarie.Client!=null)
        {
            this.Success = true;
        }
    }
    public bool Success { get; set; } = false;
}

