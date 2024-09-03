
using AutoMapper;
using ServiceClock_BackEnd.Api.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.Api.UseCases.Company.PatchCompany;
using ServiceClock_BackEnd.Application.UseCases.CreateCompany;
using ServiceClock_BackEnd.Application.UseCases.PatchCompany;
using System;

namespace ServiceClock_BackEnd.Api.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<CreateCompanyRequest, CreateCompanyUseCaseRequest>();
        CreateMap<PatchCompanyRequest, PatchCompanyUseCaseRequest>();
    }
}

