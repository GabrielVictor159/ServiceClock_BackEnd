﻿
using Newtonsoft.Json;

namespace ServiceClock_BackEnd.Api.UseCases.Company.Login;

public class LoginCompanyRequest
{
    [JsonProperty("Email")]
    public string Email { get; set; } = "";
    [JsonProperty("Password")]
    public string Password { get; set; } = "";
}

