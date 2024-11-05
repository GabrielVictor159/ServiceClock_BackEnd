
using ServiceClock_BackEnd.Application.Boundaries.Services;

namespace ServiceClock_BackEnd.UseCases.Services.CreateService;

public class CreateServiceResponse
{
    public CreateServiceResponse(CreateServiceBoundarie boundarie)
    {
        if (boundarie.Service != null)
        {
            this.Id= boundarie.Service.Id;
        }
    }
    public Guid? Id { get; set; }
}

