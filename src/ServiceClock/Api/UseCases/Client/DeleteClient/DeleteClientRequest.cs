
using Newtonsoft.Json;

namespace ServiceClock_BackEnd.Api.UseCases.Client.DeleteClient;

public class DeleteClientRequest
{
    [JsonProperty("ClientId")]
    public Guid ClientId { get; set; }
}

