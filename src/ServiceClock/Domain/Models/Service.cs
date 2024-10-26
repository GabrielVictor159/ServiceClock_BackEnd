
using ServiceClock_BackEnd.Domain.Validations;

namespace ServiceClock_BackEnd.Domain.Models;

public class Service : Entity<Service,ServiceValidator>
{
    public Service() 
        : base(new ())
    {
    }

    public Guid Id { get; set; } 
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Address { get; set; } = "";
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string Country { get; set; } = "";
    public string PostalCode { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Guid CompanyId { get; set; }

}

