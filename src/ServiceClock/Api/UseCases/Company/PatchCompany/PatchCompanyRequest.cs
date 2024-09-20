
using Newtonsoft.Json;

namespace ServiceClock_BackEnd.Api.UseCases.Company.PatchCompany;

public class PatchCompanyRequest
{
    [JsonProperty("Name")]
    public string Name { get; set; } = "";
    [JsonProperty("Password")]
    public string Password { get; set; } = "";
    [JsonProperty("RegistrationNumber")]
    public string RegistrationNumber { get; set; } = "";
    [JsonProperty("Address")]
    public string Address { get; set; } = "";
    [JsonProperty("Image")]
    public string Image { get; set; } = "";
    [JsonProperty("ImageName")]
    public string ImageName { get; set; } = "";
    [JsonProperty("City")]
    public string City { get; set; } = "";
    [JsonProperty("State")]
    public string State { get; set; } = "";
    [JsonProperty("PostalCode")]
    public string PostalCode { get; set; } = "";
    [JsonProperty("PhoneNumber")]
    public string PhoneNumber { get; set; } = "";
}

