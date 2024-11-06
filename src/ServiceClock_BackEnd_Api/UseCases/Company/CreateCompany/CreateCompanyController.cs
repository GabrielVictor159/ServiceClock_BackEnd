using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Boundaries.Company;
using ServiceClock_BackEnd.Application.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd_Application.Interfaces;

namespace ServiceClock_BackEnd_Api.UseCases.Company.CreateCompany;
[Route("api/[controller]")]
[ApiController]
public class CreateCompanyController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IOutputPort<CreateCompanyBoundarie> presenter;
    private readonly ICreateCompanyUseCase useCase;

    public CreateCompanyController
        (IMapper mapper,
        IOutputPort<CreateCompanyBoundarie> presenter,
        ICreateCompanyUseCase useCase)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.useCase = useCase;
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Run([FromBody] CreateCompanyRequest request)
    {

        if (request != null)
        {
            var requestUseCase = this.mapper.Map<CreateCompanyUseCaseRequest>(request);
            this.useCase.Execute(requestUseCase);
        }
        return this.presenter.ViewModel;

    }
}
