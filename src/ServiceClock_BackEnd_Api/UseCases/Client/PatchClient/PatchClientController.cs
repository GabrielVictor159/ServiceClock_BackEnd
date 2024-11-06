using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Boundaries.Client;
using ServiceClock_BackEnd.Application.UseCases.Client.PatchClient;
using ServiceClock_BackEnd.UseCases.Client.PatchClient;
using ServiceClock_BackEnd_Application.Interfaces;

namespace ServiceClock_BackEnd_Api.UseCases.Client.PatchClient;
[Route("api/[controller]")]
[ApiController]
public class PatchClientController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IOutputPort<PatchClientBoundarie> presenter;
    private readonly IPatchClientUseCase useCase;

    public PatchClientController
        (IMapper mapper, 
        IOutputPort<PatchClientBoundarie> presenter, 
        IPatchClientUseCase useCase)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.useCase = useCase;
    }

    [HttpPatch]
    public IActionResult Run([FromBody] PatchClientRequest request)
    {
            if (request != null)
            {
                var requestUseCase = this.mapper.Map<PatchClientUseCaseRequest>(request);
                requestUseCase.Client.Id = Guid.Parse(User.FindFirst("User_Id")!.Value);
                this.useCase.Execute(requestUseCase);
            }
            return this.presenter.ViewModel;
    }
}
