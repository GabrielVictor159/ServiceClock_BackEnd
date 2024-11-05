
using AutoMapper;
using ServiceClock_BackEnd.UseCases.Appointment.AlterStateAppointment;
using ServiceClock_BackEnd.UseCases.Appointment.RequestAppointment;
using ServiceClock_BackEnd.UseCases.Client.CreateClient;
using ServiceClock_BackEnd.UseCases.Client.PatchClient;
using ServiceClock_BackEnd.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.UseCases.Company.PatchCompany;
using ServiceClock_BackEnd.UseCases.Messages.CreateMessage;
using ServiceClock_BackEnd.UseCases.Services.CreateService;
using ServiceClock_BackEnd.UseCases.Services.DeleteService;
using ServiceClock_BackEnd.UseCases.Services.EditService;
using ServiceClock_BackEnd.Application.UseCases.Appointment.AlterStateAppointment;
using ServiceClock_BackEnd.Application.UseCases.Appointment.RequestAppointment;
using ServiceClock_BackEnd.Application.UseCases.Client.CreateClient;
using ServiceClock_BackEnd.Application.UseCases.Client.PatchClient;
using ServiceClock_BackEnd.Application.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany;
using ServiceClock_BackEnd.Application.UseCases.Messages.CreateMessage;
using ServiceClock_BackEnd.Application.UseCases.Services.CreateService;
using ServiceClock_BackEnd.Application.UseCases.Services.DeleteService;
using ServiceClock_BackEnd.Domain.Models;
using System;

namespace ServiceClock_BackEnd.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<CreateCompanyRequest, CreateCompanyUseCaseRequest>();
        CreateMap<PatchCompanyRequest, PatchCompanyUseCaseRequest>();
        CreateMap<CreateClientRequest, CreateClientUseCaseRequest>();
        CreateMap<PatchClientRequest, PatchClientUseCaseRequest>();
        CreateMap<CreateServiceRequest, CreateServiceUseCaseRequest>();
        CreateMap<DeleteServiceRequest, DeleteServiceUseCaseRequest>();
        CreateMap<RequestAppointmentRequest, RequestAppointmentUseCaseRequest>();
        CreateMap<CreateMessageRequest, CreateMessageUseCaseRequest>();
        CreateMap<AlterStateAppointmentRequest, AlterStateAppointmentUseCaseRequest>();
        CreateMap<EditServiceRequest, Service>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}

