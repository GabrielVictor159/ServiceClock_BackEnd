
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Application.UseCases.Services.CreateService;

public class CreateServiceUseCaseRequest
{
    public CreateServiceUseCaseRequest(string name, string description, string address, string city, string state, string country, string postalCode)
    {
        this.Service = new Service()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Address = address,
            City = city,
            State = state,
            Country = country,
            PostalCode = postalCode,
            CreatedAt = DateTime.Now
        };
    }
    public Service? Service { get; set; }
}
    
    
