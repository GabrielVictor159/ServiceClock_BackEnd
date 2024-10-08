﻿using Newtonsoft.Json;

namespace ServiceClock_BackEnd.Api.UseCases.Client.PatchClient;

public class PatchClientRequest
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
    [JsonProperty("BirthDate")]
    public DateTime BirthDate { get; set; } = DateTime.MinValue;
}

