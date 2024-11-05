using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Boundaries.Company;
using ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany;
using ServiceClock_BackEnd.UseCases.Company.PatchCompany;
using ServiceClock_BackEnd_Application.Interfaces;

namespace ServiceClock_BackEnd_Api.UseCases.Company.PatchCompany;
[Route("api/[controller]")]
[ApiController]
public class PatchCompanyController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IOutputPort<PatchCompanyBoundarie> presenter;
    private readonly IPatchCompanyUseCase useCase;

    public PatchCompanyController
        (IMapper mapper,
        IOutputPort<PatchCompanyBoundarie> presenter,
        IPatchCompanyUseCase useCase)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.useCase = useCase;
    }

    [HttpPatch]
    public IActionResult Run([FromBody] PatchCompanyRequest request)
    {
        if (request != null)
        {
            var requestUseCase = this.mapper.Map<PatchCompanyUseCaseRequest>(request);
            requestUseCase.Company.Id = Guid.Parse(User.FindFirst("User_Id")!.Value);
            this.useCase.Execute(requestUseCase);
        }
        return this.presenter.ViewModel;
    }
}
