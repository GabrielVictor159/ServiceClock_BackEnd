
namespace ServiceClock_BackEnd.Application.UseCases.Client.PatchClient;

public class PatchClientUseCaseRequest
{

    public PatchClientUseCaseRequest(string name, string password, string email, string phoneNumber, string address, string city, string state, string country, string postalCode, DateTime birthDate)
    {
        this.Client = new Domain.Models.Client()
        {
            Id = Guid.Empty,
            Name = name,
            Password = password,
            Email = email,
            PhoneNumber = phoneNumber,
            Address = address,
            City = city,
            State = state,
            Country = country,
            PostalCode = postalCode,
            BirthDate = birthDate
        };
    }
    public string Image { get; set; } = "";
    public Domain.Models.Client Client { get; set; }

}

