
using ServiceClock_BackEnd.Application.Boundaries.Services;

namespace ServiceClock_BackEnd.Api.UseCases.Services.DeleteService;

public class DeleteServiceResponse : ResponseCore
{
    public DeleteServiceResponse(DeleteServiceBoundarie boundarie)
        : base("Service")
    {
        this.Sucess = boundarie.Sucess;
    }
    public bool Sucess { get; set; } = false;
}

