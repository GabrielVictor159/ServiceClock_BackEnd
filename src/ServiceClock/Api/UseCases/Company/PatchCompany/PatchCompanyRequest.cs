
using Newtonsoft.Json;

namespace ServiceClock_BackEnd.Api.UseCases.Company.PatchCompany;

public class PatchCompanyRequest
{
    public string Name { get; set; } = "";
    public string Password { get; set; } = "";
    public string RegistrationNumber { get; set; } = "";
    public string Address { get; set; } = "";
    public string Image { get; set; } = "";
    public string ImageName { get; set; } = "";
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string PostalCode { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
}

