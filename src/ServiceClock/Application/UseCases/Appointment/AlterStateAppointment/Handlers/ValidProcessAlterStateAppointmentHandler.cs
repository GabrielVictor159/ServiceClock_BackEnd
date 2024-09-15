using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Application.UseCases.Appointment.AlterStateAppointment.Handlers;
public class ValidProcessAlterStateAppointmentHandler : Handler<AlterStateAppointmentUseCaseRequest>
{
    private readonly IRepository<Domain.Models.Client> clientRepository;
    private readonly IRepository<Domain.Models.Company> companyRepository;
    private readonly IRepository<Domain.Models.Service> serviceRepository;
    private readonly IRepository<Domain.Models.Appointment> appointmentRepository;
    private readonly INotificationService notificationService;
    public ValidProcessAlterStateAppointmentHandler
        (ILogService logService,
        IRepository<Domain.Models.Client> clientRepository,
        IRepository<Domain.Models.Company> companyRepository,
        IRepository<Domain.Models.Service> serviceRepository,
        IRepository<Domain.Models.Appointment> appointmentRepository,
        INotificationService notificationService) 
        : base(logService)
    {
        this.clientRepository = clientRepository;
        this.companyRepository = companyRepository;
        this.serviceRepository = serviceRepository;
        this.appointmentRepository = appointmentRepository;
        this.notificationService = notificationService;
    }

    public override void ProcessRequest(AlterStateAppointmentUseCaseRequest request)
    {
        if(request.Status == Domain.Enums.AppointmentStatus.PendingApproval)
        {
            notificationService.AddNotification("Status not allowed", "Status não permitido");
            return;
        }
        request.Appointment = appointmentRepository.GetById(request.AppointmentId.ToString());
        if (request.Appointment == null)
        {
            notificationService.AddNotification("Appointment not found", "Agendamento não encontrado");
            return;
        }
        if (request.Appointment.Status == Domain.Enums.AppointmentStatus.Completed
            || request.Appointment.Status == Domain.Enums.AppointmentStatus.Canceled
            || request.Appointment.Status == Domain.Enums.AppointmentStatus.Rejected)
        {
            notificationService.AddNotification("Appointment already completed, canceled or rejected", "Agendamento já completado, cancelado ou rejeitado");
            return;
        }

        var service = serviceRepository.Find(e => e.Id == request.Appointment.ServiceId).FirstOrDefault();
        if (service == null)
        {
            notificationService.AddNotification("Service not found", "Serviço não encontrado");
            return;
        }

        if(request.Appointment.ClientId!=request.UserId && service.CompanyId!=request.UserId)
        {
            notificationService.AddNotification("User is not allowed to alter this appointment", "Usuário não tem permissão para alterar este agendamento");
            return;
        }

        switch (request.UserType)
        {
            case "Client":
                if(request.Status== Domain.Enums.AppointmentStatus.Canceled || request.Status == Domain.Enums.AppointmentStatus.Approved)
                {
                    notificationService.AddNotification("Client can't cancel a confirmed appointment", "Cliente não pode cancelar, confirmar ou completar um agendamento");
                    return;
                }
                break;
            case "Company":
                if(request.Status==Domain.Enums.AppointmentStatus.Canceled || request.Status == Domain.Enums.AppointmentStatus.Completed)
                {
                    notificationService.AddNotification("Company can't cancel or complete an appointment", "Empresa não pode cancelar ou completar um agendamento");
                    return;
                }
                break;
            case "System":
                break;
            default:
                notificationService.AddNotification("User type not found", "Tipo de usuário não encontrado");
                return;
        }

        sucessor?.ProcessRequest(request);
    }
}
