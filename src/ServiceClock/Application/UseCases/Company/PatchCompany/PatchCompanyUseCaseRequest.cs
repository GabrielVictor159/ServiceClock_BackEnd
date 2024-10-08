﻿using Newtonsoft.Json;
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany;

public class PatchCompanyUseCaseRequest
{
    public PatchCompanyUseCaseRequest
        (string name, string password, string registrationNumber, string address, string city, string state,
        string postalCode, string phoneNumber)
    {
        Company = new Domain.Models.Company
        {
            Id = Guid.Empty,
            Name = name,
            Password = password,
            RegistrationNumber = registrationNumber,
            Address = address,
            City = city,
            State = state,
            PostalCode = postalCode,
            PhoneNumber = phoneNumber,
        };
    }
    public string Image { get; set; } = "";
    public string ImageName { get; set; } = "";
    public Domain.Models.Company Company { get; set; }
}

