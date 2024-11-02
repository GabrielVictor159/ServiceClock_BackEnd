using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs;
using Microsoft.OpenApi.Models;
using ServiceClock_BackEnd.Api.Filters;
using ServiceClock_BackEnd.Api.UseCases.Appointment.RequestAppointment;
using ServiceClock_BackEnd.Api.Validator.Http;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.UseCases.Appointment.RequestAppointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using ServiceClock_BackEnd.Api.Helpers.Hateoas;
using System.Linq.Expressions;

namespace ServiceClock_BackEnd.Api.UseCases.Appointment.ListAppointment;
public class ListAppointment : UseCaseCore
{
    private readonly IRepository<Domain.Models.Appointment> appointmentRepository;
    private readonly IRepository<Domain.Models.Service> serviceRepository;
    private readonly IRepository<Domain.Models.Client> clientRepository;
    public ListAppointment
        (HttpRequestValidator httpRequestValidator, 
        NotificationMiddleware middleware,
        IRepository<Domain.Models.Appointment> appointmentRepository,
        IRepository<Domain.Models.Service> serviceRepository,
        IRepository<Domain.Models.Client> clientRepository) 
        : base(httpRequestValidator.AddValidator(new AuthorizationValidator()), middleware)
    {
        this.appointmentRepository = appointmentRepository;
        this.serviceRepository = serviceRepository;
        this.clientRepository = clientRepository;
    }
    [FunctionName("ListAppointment")]
    [OpenApiOperation(operationId: "ListAppointment", tags: new[] { "Appointment" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ListAppointmentRequest), Description = "Request body containing service information.")]
    [OpenApiSecurity("bearer",
                     SecuritySchemeType.ApiKey,
                     In = OpenApiSecurityLocationType.Query,
                     Name = "code")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response with the created company details.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "The Bad Request response in case of invalid input.")]
    [Hateoas("Appointment","search","/ListAppointment","POST",typeof(ListAppointmentRequest))]
    public async Task<IActionResult> Run(
       [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        return await Execute(req, async (ListAppointmentRequest request) =>
        {
            var userId = Guid.Parse(httpRequestValidator.Claims.Where(e => e.Type == "User_Id").First().Value);
            var userRule = httpRequestValidator.Claims.Where(e => e.Type == "User_Rule").First().Value;

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

            var clients = clientRepository.Find(e => clientIds.Contains(e.Id) && e.Active==true).ToList();

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
        });
    }

}
