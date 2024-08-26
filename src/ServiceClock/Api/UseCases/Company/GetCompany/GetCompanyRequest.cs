
using Newtonsoft.Json;
using ServiceClock_BackEnd.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceClock_BackEnd.Api.UseCases.Company.GetCompany;

public class GetCompanyRequest
{
    [JsonProperty("Id")]
    public Guid Id { get; set; } = Guid.Empty;
    [JsonProperty("Name")]
    public string Name { get; set; } = "";
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
    [JsonProperty("IndexPage")]
    [Range(1, int.MaxValue, ErrorMessage = "IndexPage must be greater than or equal to 1.")]
    public int IndexPage { get; set; } = 1;
    [JsonProperty("PageSize")]
    public PageSize PageSize {get; set; } = PageSize.Minimal;
}

