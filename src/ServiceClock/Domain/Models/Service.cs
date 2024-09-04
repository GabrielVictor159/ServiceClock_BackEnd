﻿
namespace ServiceClock_BackEnd.Domain.Models;

public class Service
{
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
    public Company? Company { get; set; }

}

