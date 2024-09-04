
using Newtonsoft.Json;
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Application.UseCases.Company.CreateCompany;

public class CreateCompanyUseCaseRequest
{
    public CreateCompanyUseCaseRequest
        (string name, string password, string registrationNumber, string address, string city, string state, string country,
        string postalCode, string phoneNumber, string email)
    {
        Company = new Company
        {
            Id = Guid.NewGuid(),
            Name = name,
            Password = password,
            RegistrationNumber = registrationNumber,
            Address = address,
            City = city,
            State = state,
            Country = country,
            PostalCode = postalCode,
            PhoneNumber = phoneNumber,
            Email = email,
            EstablishedDate = DateTime.Now
        };
    }

    public Company? Company { get; set; }

}

