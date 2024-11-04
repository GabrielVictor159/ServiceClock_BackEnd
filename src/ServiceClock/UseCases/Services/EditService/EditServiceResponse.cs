
using ServiceClock_BackEnd.Application.Boundaries.Services;

namespace ServiceClock_BackEnd.Api.UseCases.Services.EditService;

public class EditServiceResponse
{
    public EditServiceResponse(EditServiceBoundarie boundarie)
    {
        if(boundarie.Service!=null)
        {
            this.Sucess = true;
        }
    }
    public bool Sucess = false;
}

