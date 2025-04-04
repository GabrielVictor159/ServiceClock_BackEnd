﻿
using Newtonsoft.Json;

namespace ServiceClock_BackEnd.UseCases.Company.CreateCompany;

public class CreateCompanyRequest
{
    [JsonProperty("Name")]
    public string Name { get; set; } = "";
    [JsonProperty("Password")]
    public string Password { get; set; } = "";
    [JsonProperty("RegistrationNumber")]
    public string RegistrationNumber { get; set; } = "";
    [JsonProperty("Address")]
    public string Address { get; set; } = "";
    [JsonProperty("City")]
    public string City { get; set; } = "";
    [JsonProperty("State")]
    public string State { get; set; } = "";
    [JsonProperty("Country")]
    public string Country { get; set; } = "";
    [JsonProperty("PostalCode")]
    public string PostalCode { get; set; } = "";
    [JsonProperty("PhoneNumber")]
    public string PhoneNumber { get; set; } = "";
    [JsonProperty("Email")]
    public string Email { get; set; } = "";
}

