
using ServiceClock_BackEnd.Application.Boundaries.Services;

namespace ServiceClock_BackEnd.Api.UseCases.Services.CreateService;

public class CreateServiceResponse : ResponseCore
{
    public CreateServiceResponse(CreateServiceBoundarie boundarie)
        : base("Service")
    {
        if (boundarie.Service != null)
        {
            this.Id= boundarie.Service.Id;
        }
    }
    public Guid? Id { get; set; }
}

