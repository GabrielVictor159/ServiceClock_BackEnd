using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Api.UseCases.Client.CreateClient;
public class CreateClientRequest
{
    [JsonProperty("Name")]
    public string Name { get; set; } = "";
    [JsonProperty("Password")]
    public string Password { get; set; } = "";
    [JsonProperty("Email")]
    public string Email { get; set; } = "";
    [JsonProperty("PhoneNumber")]
    public string PhoneNumber { get; set; } = "";
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
    [JsonProperty("BirthDate")]
    public DateTime BirthDate { get; set; } = DateTime.MinValue;
}
