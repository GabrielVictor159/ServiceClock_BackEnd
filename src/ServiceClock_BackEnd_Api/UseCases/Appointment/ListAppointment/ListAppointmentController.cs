using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Helpers.Hateoas;
using ServiceClock_BackEnd.UseCases.Appointment.ListAppointment;

namespace ServiceClock_BackEnd_Api.UseCases.Appointment.ListAppointment;
[Route("api/[controller]")]
[ApiController]
public class ListAppointmentController : ControllerBase
{
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Appointment> appointmentRepository;
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Service> serviceRepository;
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Client> clientRepository;

    public ListAppointmentController
        (IRepository<ServiceClock_BackEnd.Domain.Models.Appointment> appointmentRepository,
        IRepository<ServiceClock_BackEnd.Domain.Models.Service> serviceRepository,
        IRepository<ServiceClock_BackEnd.Domain.Models.Client> clientRepository)
    {
        this.appointmentRepository = appointmentRepository;
        this.serviceRepository = serviceRepository;
        this.clientRepository = clientRepository;
    }

    [HttpPost]
    [Hateoas("Appointment", "search", "/ListAppointment", "POST", typeof(ListAppointmentRequest))]
    public IActionResult Run(ListAppointmentRequest request)
    {
        var userId = Guid.Parse(User.FindFirst("User_Id")!.Value);
        var userRule = User.FindFirst("User_Rule")!.Value;

        if (userRule == "Client")
        {
            request.ClientId = userId;
        }

        var service = this.serviceRepository.Find(e => e.Id == request.ServiceId).FirstOrDefault();
        if (request.ServiceId == Guid.Empty || service == null)
        {
            return new BadRequestObjectResult("Serviço não encontrado");
        }

        if (userRule == "Company")
        {
            if (service.CompanyId != userId)
            {
                return new ForbidResult("Você não tem permissão para ver os agendamentos deste serviço");
            }
        }

        var appointments = this.appointmentRepository.Find(e =>
            (request.Id == Guid.Empty || e.Id == request.Id) &&
            (request.ClientId == Guid.Empty || e.ClientId == request.ClientId) &&
            (e.ServiceId == request.ServiceId) &&
            (request.Status == null || e.Status == request.Status) &&
            (e.Date >= request.MinDate) &&
            (e.Date <= request.MaxDate), request.IndexPage, ((int)request.PageSize))
            .ToList();

        var clientIds = appointments.Select(a => a.ClientId).Distinct().ToList();

        var clients = clientRepository.Find(e => clientIds.Contains(e.Id) && e.Active == true).ToList();

        var clientsDict = clients.ToDictionary(c => c.Id);

        var result = appointments.Select(a => new
        {
            Id = a.Id,
            ClientId = a.ClientId,
            ServiceId = a.ServiceId,
            Date = a.Date,
            Status = a.Status,
            Description = a.Description,
            CreatedAt = a.CreatedAt,
            Client = clientsDict.TryGetValue(a.ClientId, out var client) ? new
            {
                Name = client.Name,
                Email = client.Email,
                ClientImage = client.ClientImage,
                Address = client.Address,
                City = client.City,
                State = client.State,
                PostalCode = client.PostalCode,
            } : null
        });

        return new OkObjectResult(new
        {
            Appointments = result,
            _links = HateoasScheme.Instance.GetLinks("Appointment")
        });

    }
}
