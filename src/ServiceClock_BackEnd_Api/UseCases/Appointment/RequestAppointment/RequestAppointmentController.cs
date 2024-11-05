using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Boundaries.Appointment;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.UseCases.Appointment.RequestAppointment;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Helpers.Hateoas;
using ServiceClock_BackEnd.UseCases.Appointment.RequestAppointment;
using ServiceClock_BackEnd_Application.Interfaces;

namespace ServiceClock_BackEnd_Api.UseCases.Appointment.RequestAppointment;
[Route("api/[controller]")]
[ApiController]
public class RequestAppointmentController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IOutputPort<RequestAppointmentBoundarie> presenter;
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Client> clientReposiotory;
    private IRequestAppointmentUseCase useCase;

    public RequestAppointmentController
        (IMapper mapper,
        IOutputPort<RequestAppointmentBoundarie> presenter,
        IRepository<ServiceClock_BackEnd.Domain.Models.Client> clientReposiotory,
        IRequestAppointmentUseCase useCase)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.clientReposiotory = clientReposiotory;
        this.useCase = useCase;
    }

    [HttpPost]
    [Hateoas("Appointment", "create", "/RequestAppointment", "POST", typeof(RequestAppointmentRequest))]
    public IActionResult Run([FromBody] RequestAppointmentRequest request)
    {
        if (User.FindFirst("User_Rule")!.Value != "Client")
        {

            return new UnauthorizedObjectResult("The user does not have permission to perform this action.");
        }
        var userId = Guid.Parse(User.FindFirst("User_Id")!.Value);
        var client = this.clientReposiotory.Find(e => e.Id == userId && e.Active == true).FirstOrDefault();
        if (request != null && client != null)
        {
            request.clientId = client.Id;
            var requestUseCase = this.mapper.Map<RequestAppointmentUseCaseRequest>(request);
            requestUseCase.Client = client;
            this.useCase.Execute(requestUseCase);
        }
        return this.presenter.ViewModel;
    }
}
