
using Newtonsoft.Json;

namespace ServiceClock_BackEnd.UseCases.Client.DeleteClient;

public class DeleteClientRequest
{
    [JsonProperty("ClientId")]
    public Guid ClientId { get; set; }
}

