using ServiceClock_BackEnd.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Domain.Models;
public class Client : Entity<Client,ClientValidator>
{
    public Client() 
        : base(new())
    {
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Password { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string Email { get; set; } = "";
    public string Address { get; set; } = "";
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string Country { get; set; } = "";
    public string PostalCode { get; set; } = "";
    public DateTime BirthDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool Active { get; set; } = true;

    public Guid? CompanyId { get; set; } 
    public Company? Company { get; set; }

}
