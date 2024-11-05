
using Newtonsoft.Json;

namespace ServiceClock_BackEnd.UseCases.Client.Login;

public class LoginClientRequest
{
    [JsonProperty("Email")]
    public string Email { get; set; } = "";
    [JsonProperty("Password")]
    public string Password { get; set; } = "";
}
