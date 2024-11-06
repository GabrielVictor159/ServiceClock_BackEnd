using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Boundaries.Services;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.UseCases.Services.EditService;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.UseCases.Services.EditService;
using ServiceClock_BackEnd_Application.Interfaces;

namespace ServiceClock_BackEnd_Api.UseCases.Services.EditService;
[Route("api/[controller]")]
[ApiController]
public class EditServiceController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IOutputPort<EditServiceBoundarie> presenter;
    private readonly IRepository<Service> repository;
    private IEditServiceUseCase useCase;

    public EditServiceController
        (IMapper mapper,
        IOutputPort<EditServiceBoundarie> presenter,
        IRepository<Service> repository,
        IEditServiceUseCase useCase)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.repository = repository;
        this.useCase = useCase;
    }

    [HttpPatch]
    public IActionResult Run([FromBody] EditServiceRequest request)
    {
        var UserId = Guid.Parse(User.FindFirst("User_Id")!.Value);
        var UserType = User.FindFirst("User_Rule")!.Value;
        if (request != null)
        {
            var serviceExisting = repository.FindSingle(e => e.Id == request.Id);
            if (serviceExisting == null)
            {
                return new NotFoundObjectResult("Serviço não encontrado");
            }
            if (!serviceExisting.CompanyId.Equals(UserId))
            {
                return new ForbidResult();
            }

            var newService = mapper.Map(request, serviceExisting);

            if (newService == null)
            {
                return new BadRequestObjectResult("Não foi possivel converter o request para o dominio");
            }

            var useCaseRequest = new EditServiceUseCaseRequest(newService);

            this.useCase.Execute(useCaseRequest);
        }
        return this.presenter.ViewModel;
    }
}
