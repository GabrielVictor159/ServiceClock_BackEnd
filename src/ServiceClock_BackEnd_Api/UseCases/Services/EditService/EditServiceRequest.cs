﻿
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ServiceClock_BackEnd.UseCases.Services.EditService;

public class EditServiceRequest
{
    [JsonProperty("Id")]
    [Required]
    public Guid Id { get; set; }
    [JsonProperty("Name")]
    public string? Name { get; set; }
    [JsonProperty("Description")]
    public string? Description { get; set; } 
    [JsonProperty("Address")]
    public string? Address { get; set; }
    [JsonProperty("City")]
    public string? City { get; set; }
    [JsonProperty("State")]
    public string? State { get; set; }
    [JsonProperty("Country")]
    public string? Country { get; set; }
    [JsonProperty("PostalCode")]
    public string? PostalCode { get; set; }

}

