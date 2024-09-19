
using Newtonsoft.Json;

namespace ServiceClock_BackEnd.Api.UseCases.Messages.ListMessage;

public class ListMessageRequest
{
    [JsonProperty("ClientId")]
    public Guid ClientId { get; set; }
    [JsonProperty("CompanyId")]
    public Guid CompanyId { get; set; }
}

