using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany;

public class PatchCompanyUseCaseRequest
{
    public PatchCompanyUseCaseRequest
        (string name, string password, string registrationNumber, string address, string city, string state, string country,
        string postalCode, string phoneNumber)
    {
        Company = new Company
        {
            Id = Guid.Empty,
            Name = name,
            Password = password,
            RegistrationNumber = registrationNumber,
            Address = address,
            City = city,
            State = state,
            Country = country,
            PostalCode = postalCode,
            PhoneNumber = phoneNumber,
        };
    }

    public Company Company { get; set; }
}

