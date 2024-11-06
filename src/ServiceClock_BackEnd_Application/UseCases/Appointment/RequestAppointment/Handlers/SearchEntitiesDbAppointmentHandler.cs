using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Application.UseCases.Appointment.RequestAppointment.Handlers;
public class SearchEntitiesDbAppointmentHandler : Handler<RequestAppointmentUseCaseRequest>
{
    private readonly IRepository<Service> serviceRepository;
    private readonly IRepository<Domain.Models.Client> clientRepository;
    private readonly INotificationService notificationService;

    public SearchEntitiesDbAppointmentHandler
        (IRepository<Service> serviceRepository, 
        IRepository<Domain.Models.Client> clientRepository, 
        INotificationService notificationService, 
        ILogService logService) 
        : base(logService)
    {
        this.serviceRepository = serviceRepository;
        this.clientRepository = clientRepository;
        this.notificationService = notificationService;
    }

    public override void ProcessRequest(RequestAppointmentUseCaseRequest request)
    {
        var service = serviceRepository.Find(e=>e.Id==request.Appointment.ServiceId).FirstOrDefault();
        if(service==null)
        {
            notificationService.AddNotification("Service not found", "Serviço não encontrado");
            return;
        }

        var client = clientRepository.Find(e => e.Id == request.Appointment.ClientId).FirstOrDefault();
        if (client == null)
        {
            notificationService.AddNotification("Client not found", "Cliente não encontrado");
            return;
        }

        if(client.CompanyId!=service.CompanyId)
        {
            notificationService.AddNotification("Client and service are not from the same company", "Cliente e serviço não são da mesma empresa");
            return;
        }


        sucessor?.ProcessRequest(request);
    }
}
