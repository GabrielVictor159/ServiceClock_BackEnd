﻿using ServiceClock_BackEnd.Application.Boundaries.Appointment;


namespace ServiceClock_BackEnd.UseCases.Appointment.RequestAppointment;
public class RequestAppointmentResponse
{
    public RequestAppointmentResponse(RequestAppointmentBoundarie boundarie)
    {
        if(boundarie.Appointment!=null)
        {
            this.AppointmentId = boundarie.Appointment.Id;
            this.Success = true;
            this.Appointment = new
            {
                Id = boundarie.Appointment.Id,
                ClientId = boundarie.Appointment.ClientId,
                ServiceId = boundarie.Appointment.ServiceId,
                Date = boundarie.Appointment.Date,
                Status = boundarie.Appointment.Status,
                Description = boundarie.Appointment.Description,
                CreatedAt = boundarie.Appointment.CreatedAt,
                Client = boundarie.Client != null ? new
                {
                    Name = boundarie.Client.Name,
                    Email = boundarie.Client.Email,
                    ClientImage = boundarie.Client.ClientImage,
                    Address = boundarie.Client.Address,
                    City = boundarie.Client.City,
                    State = boundarie.Client.State,
                    PostalCode = boundarie.Client.PostalCode,
                } : null
            };
        }
    }
    public Guid AppointmentId { get; set; }
    public bool Success { get; set; } = false;
    public object? Appointment { get; set; } = null;
}
