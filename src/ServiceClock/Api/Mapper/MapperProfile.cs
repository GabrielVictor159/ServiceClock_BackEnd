
using AutoMapper;
using ServiceClock_BackEnd.Api.UseCases.Appointment.RequestAppointment;
using ServiceClock_BackEnd.Api.UseCases.Client.CreateClient;
using ServiceClock_BackEnd.Api.UseCases.Client.PatchClient;
using ServiceClock_BackEnd.Api.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.Api.UseCases.Company.PatchCompany;
using ServiceClock_BackEnd.Api.UseCases.Messages.CreateMessage;
using ServiceClock_BackEnd.Api.UseCases.Services.CreateService;
using ServiceClock_BackEnd.Api.UseCases.Services.DeleteService;
using ServiceClock_BackEnd.Application.UseCases.Appointment.RequestAppointment;
using ServiceClock_BackEnd.Application.UseCases.Client.CreateClient;
using ServiceClock_BackEnd.Application.UseCases.Client.PatchClient;
using ServiceClock_BackEnd.Application.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany;
using ServiceClock_BackEnd.Application.UseCases.Messages.CreateMessage;
using ServiceClock_BackEnd.Application.UseCases.Services.CreateService;
using ServiceClock_BackEnd.Application.UseCases.Services.DeleteService;
using System;

namespace ServiceClock_BackEnd.Api.Mapper;

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
    }
}

