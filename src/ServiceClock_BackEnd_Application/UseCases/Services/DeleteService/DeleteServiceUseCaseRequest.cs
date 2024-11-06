
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Application.UseCases.Services.DeleteService;

public class DeleteServiceUseCaseRequest
{
    public DeleteServiceUseCaseRequest(Guid serviceId)
    {
        ServiceId = serviceId;
    }

    public Guid ServiceId { get; set; }
    public Guid CompanyId { get; set; }
    public List<Domain.Models.Appointment> appointments { get; set; } = new();
    public Service? Service { get; set; }
    public bool IsDeleted { get; set; } = false;
}

