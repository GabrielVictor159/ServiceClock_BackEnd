using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Boundaries.Services;
using ServiceClock_BackEnd.Application.UseCases.Services.CreateService;
using ServiceClock_BackEnd.Helpers.Hateoas;
using ServiceClock_BackEnd.UseCases.Services.CreateService;
using ServiceClock_BackEnd_Application.Interfaces;

namespace ServiceClock_BackEnd_Api.UseCases.Services.CreateService;
[Route("api/[controller]")]
[ApiController]
public class CreateServiceController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IOutputPort<CreateServiceBoundarie> presenter;
    private ICreateServiceUseCase useCase;

    public CreateServiceController
        (IMapper mapper,
        IOutputPort<CreateServiceBoundarie> presenter,
        ICreateServiceUseCase useCase)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.useCase = useCase;
    }

    [HttpPost]
    [Hateoas("Service", "create", "/CreateService", "POST", typeof(CreateServiceRequest))]
    public IActionResult Run(CreateServiceRequest request)
    {
        var UserId = Guid.Parse(User.FindFirst("User_Id")!.Value);
        var UserType = User.FindFirst("User_Rule")!.Value;
        if (UserType != "Company")
        {
            return new ForbidResult("Você não tem permissão para criar um serviço");
        }
        if (request != null)
        {
            var requestUseCase = this.mapper.Map<CreateServiceUseCaseRequest>(request);
            if (requestUseCase.Service != null)
            {
                requestUseCase.Service.CompanyId = UserId;
                this.useCase.Execute(requestUseCase);
            }
        }
        return this.presenter.ViewModel;
    }
}
