using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.UseCases.Client.DeleteClient;
using ServiceClock_BackEnd.UseCases.Client.DeleteClient;

namespace ServiceClock_BackEnd_Api.UseCases.Client.DeleteClient;
[Route("api/[controller]")]
[ApiController]
public class DeleteClientController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly DeleteClientPresenter presenter;
    private IDeleteClientUseCase useCase;
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Client> clientRepository;

    public DeleteClientController
        (IMapper mapper, 
        DeleteClientPresenter presenter, 
        IDeleteClientUseCase useCase, 
        IRepository<ServiceClock_BackEnd.Domain.Models.Client> clientRepository)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.useCase = useCase;
        this.clientRepository = clientRepository;
    }

    [HttpPost]
    public IActionResult Run([FromBody] DeleteClientRequest request)
    {
            var userId = Guid.Parse(User.FindFirst("User_Id")!.Value);
            var client = this.clientRepository.Find(e => e.Id == request.ClientId && e.CompanyId == userId && e.Active == true).FirstOrDefault();

            if (client == null)
            {
                return new NotFoundObjectResult("Cliente não encontrado");
            }

            var requestUseCase = new DeleteClientUseCaseRequest(client);
            this.useCase.Execute(requestUseCase);
            return this.presenter.ViewModel;
    }
}
