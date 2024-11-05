
using Newtonsoft.Json;

namespace ServiceClock_BackEnd.UseCases.Messages.ListMessage;

public class ListMessageRequest
{
    [JsonProperty("ClientId")]
    public Guid ClientId { get; set; }
    [JsonProperty("CompanyId")]
    public Guid CompanyId { get; set; }
    [JsonProperty("MinDate")]
    public DateTime? MinDate { get; set; }
}

