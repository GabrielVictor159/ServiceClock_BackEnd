using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Boundaries.Client;
using ServiceClock_BackEnd.Application.UseCases.Client.CreateClient;
using ServiceClock_BackEnd.UseCases.Client.CreateClient;
using ServiceClock_BackEnd_Application.Interfaces;

namespace ServiceClock_BackEnd_Api.UseCases.Client.CreateClient;
[Route("api/[controller]")]
[ApiController]
public class CreateClientController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IOutputPort<CreateClientBoundarie> presenter;
    private ICreateClientUseCase useCase;

    public CreateClientController
        (IMapper mapper,
        IOutputPort<CreateClientBoundarie> presenter,
        ICreateClientUseCase useCase)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.useCase = useCase;
    }

    [HttpPost]
    public IActionResult Run([FromBody] CreateClientRequest request)
    {
        if (User.FindFirst("User_Rule")!.Value != "Company")
        {
            return new ForbidResult("Você não tem permissão para criar um usuario");
        }
        if (request != null)
        {
            var requestUseCase = this.mapper.Map<CreateClientUseCaseRequest>(request);
            if (requestUseCase.Client != null)
            {
                requestUseCase.Client.CompanyId = Guid.Parse(User.FindFirst("User_Id")!.Value);
                this.useCase.Execute(requestUseCase);
            }
        }
        return this.presenter.ViewModel;

    }
}
