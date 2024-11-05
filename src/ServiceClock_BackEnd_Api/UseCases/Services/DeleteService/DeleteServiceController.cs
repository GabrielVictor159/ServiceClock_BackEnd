using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Boundaries.Services;
using ServiceClock_BackEnd.Application.UseCases.Services.DeleteService;
using ServiceClock_BackEnd.Helpers.Hateoas;
using ServiceClock_BackEnd.UseCases.Services.DeleteService;
using ServiceClock_BackEnd_Application.Interfaces;

namespace ServiceClock_BackEnd_Api.UseCases.Services.DeleteService;
[Route("api/[controller]")]
[ApiController]
public class DeleteServiceController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IOutputPort<DeleteServiceBoundarie> presenter;
    private IDeleteServiceUseCase useCase;

    public DeleteServiceController
        (IMapper mapper,
        IOutputPort<DeleteServiceBoundarie> presenter,
        IDeleteServiceUseCase useCase)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.useCase = useCase;
    }

    [HttpPost]
    [Hateoas("Service", "delete", "/DeleteService", "POST", typeof(DeleteServiceRequest))]
    public IActionResult Run(DeleteServiceRequest request)
    {
        var UserId = Guid.Parse(User.FindFirst("User_Id")!.Value);

        if (UserId == Guid.Empty)
        {
            return new BadRequestObjectResult("Invalid token");
        }
        if (request != null)
        {
            var requestUseCase = this.mapper.Map<DeleteServiceUseCaseRequest>(request);
            requestUseCase.CompanyId = UserId;
            this.useCase.Execute(requestUseCase);

        }
        return this.presenter.ViewModel;
    }
}
