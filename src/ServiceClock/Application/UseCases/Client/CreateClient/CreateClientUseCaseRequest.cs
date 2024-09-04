using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Application.UseCases.Client.CreateClient;
public class CreateClientUseCaseRequest
{
    public CreateClientUseCaseRequest(string Name, string Password, string PhoneNumber, string Email, string Address, string City, string State, 
        string Country, string PostalCode, DateTime BirthDate)
    {
        this.Client = new Domain.Models.Client()
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Password = Password,
            PhoneNumber = PhoneNumber,
            Email = Email,
            Address = Address,
            City = City,
            State = State,
            Country = Country,
            PostalCode = PostalCode,
            BirthDate = BirthDate,
            CreatedAt = DateTime.Now,
        };
    }
    public Domain.Models.Client? Client { get; set; }
}
