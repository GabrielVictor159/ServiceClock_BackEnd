
namespace ServiceClock_BackEnd.Application.UseCases.Client.PatchClient;

public class PatchClientUseCaseRequest
{

    public PatchClientUseCaseRequest(string name, string password, string phoneNumber, string address, string city, string state, string postalCode, DateTime birthDate)
    {
        this.Client = new Domain.Models.Client()
        {
            Id = Guid.Empty,
            Name = name,
            Password = password,
            PhoneNumber = phoneNumber,
            Address = address,
            City = city,
            State = state,
            PostalCode = postalCode,
            BirthDate = birthDate
        };
    }
    public string Image { get; set; } = "";
    public string ImageName { get; set; } = "";
    public Domain.Models.Client Client { get; set; }

}

