
using AutoMapper;
using ServiceClock_BackEnd.Api.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.Application.UseCases.CreateCompany;
using System;

namespace ServiceClock_BackEnd.Api.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<CreateCompanyRequest, CreateCompanyUseCaseRequest>();
    }
}

