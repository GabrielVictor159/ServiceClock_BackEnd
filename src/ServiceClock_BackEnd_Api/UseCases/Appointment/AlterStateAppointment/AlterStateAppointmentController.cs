using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Boundaries.Appointment;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.UseCases.Appointment.AlterStateAppointment;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Filters;
using ServiceClock_BackEnd.Helpers.Hateoas;
using ServiceClock_BackEnd.UseCases.Appointment.AlterStateAppointment;
using ServiceClock_BackEnd_Application.Interfaces;
using System.Net;

namespace ServiceClock_BackEnd_Api.UseCases.Appointment.AlterStateAppointment;
[Route("api/[controller]")]
[ApiController]
public class AlterStateAppointmentController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IOutputPort<AlterStateAppointmentBoundarie> presenter;
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Client> clientRepository;
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Company> companyRepository;
    private IAlterStateAppointmentUseCase useCase;
    public AlterStateAppointmentController
        (IMapper mapper,
        IOutputPort<AlterStateAppointmentBoundarie> presenter,
        IAlterStateAppointmentUseCase useCase,
        IRepository<ServiceClock_BackEnd.Domain.Models.Client> clientRepository,
        IRepository<ServiceClock_BackEnd.Domain.Models.Company> companyRepository)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.useCase = useCase;
        this.clientRepository = clientRepository;
        this.companyRepository = companyRepository;
    }

    [HttpPost]
    [Hateoas("Appointment", "update", "/AlterStateAppointment", "POST", typeof(AlterStateAppointmentRequest))]
    public IActionResult Run([FromBody] AlterStateAppointmentRequest request)
    {

        request.UserId = Guid.Parse(User.FindFirst("User_Id")!.Value);
        request.UserType = User.FindFirst("User_Rule")!.Value;
        ServiceClock_BackEnd.Domain.Models.Client? client = null;
        ServiceClock_BackEnd.Domain.Models.Company? company = null;
        if (request.UserType == "Client")
        {
            client = this.clientRepository.Find(e => e.Id == request.UserId && e.Active == true).FirstOrDefault();
        }
        else
        {
            company = this.companyRepository.Find(e => e.Id == request.UserId).FirstOrDefault();
        }
        if (request != null && (client != null || company != null))
        {
            var requestUseCase = this.mapper.Map<AlterStateAppointmentUseCaseRequest>(request);
            this.useCase.Execute(requestUseCase);
        }
        return this.presenter.ViewModel;

    }
}
