namespace ServiceClock_BackEnd.Api.UseCases.Client.PatchClient;

public class PatchClientResponse
{
    public PatchClientResponse(Application.Boundaries.Client.PatchClientBoundarie? boundarie)
    {
        if(boundarie !=null && boundarie.Client!=null)
        {
            this.Success = true;
        }
    }
    public bool Success { get; set; } = false;
}

