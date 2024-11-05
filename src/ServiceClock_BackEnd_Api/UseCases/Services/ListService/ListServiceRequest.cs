
using Newtonsoft.Json;
using ServiceClock_BackEnd.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceClock_BackEnd.UseCases.Services.ListService;

public class ListServiceRequest
{
    [JsonProperty("Id")]
    public Guid Id { get; set; }
    [JsonProperty("Name")]
    public string Name { get; set; } = "";
    [JsonProperty("Description")]
    public string Description { get; set; } = "";
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
    [JsonProperty("IndexPage")]
    [Range(1, int.MaxValue, ErrorMessage = "IndexPage must be greater than or equal to 1.")]
    public int IndexPage { get; set; } = 1;
    [JsonProperty("PageSize")]
    public PageSize PageSize { get; set; } = PageSize.Minimal;
}


