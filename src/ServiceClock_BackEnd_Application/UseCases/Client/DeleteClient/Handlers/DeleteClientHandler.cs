
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;

namespace ServiceClock_BackEnd.Application.UseCases.Client.DeleteClient.Handlers;

public class DeleteClientHandler : Handler<DeleteClientUseCaseRequest>
{
    private readonly IRepository<Domain.Models.Client> repository;
    public DeleteClientHandler
        (ILogService logService,
        IRepository<Domain.Models.Client> repository)
        : base(logService)
    {
        this.repository = repository;
    }
    public override void ProcessRequest(DeleteClientUseCaseRequest request)
    {
        request.Client.Active = false;
        request.ClientDeleteRows = this.repository.Update(request.Client);
        sucessor?.ProcessRequest(request);
    }
}

