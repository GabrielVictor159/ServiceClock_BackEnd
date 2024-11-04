
using ServiceClock_BackEnd.Domain.Validations;

namespace ServiceClock_BackEnd.Domain.Models;

public class Company : Entity<Company, CompanyValidator>
{
    public Company()
        : base(new())
    {
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Password { get; set; } = "";
    public string RegistrationNumber { get; set; } = "";
    public string Address { get; set; } = "";
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string Country { get; set; } = "";
    public string PostalCode { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string Email { get; set; } = "";
    public string CompanyImage { get; set; } = "";
    public DateTime EstablishedDate { get; set; }
}

