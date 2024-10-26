
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Application.UseCases.Services.EditService;

public class EditServiceUseCaseRequest
{
    public EditServiceUseCaseRequest(Service service)
    {
        this.service = service;
    }
    public Service service { get; set; }

}

