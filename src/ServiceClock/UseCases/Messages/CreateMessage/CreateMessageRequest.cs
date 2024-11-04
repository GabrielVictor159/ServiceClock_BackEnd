
using Newtonsoft.Json;
using ServiceClock_BackEnd.Domain.Enums;

namespace ServiceClock_BackEnd.Api.UseCases.Messages.CreateMessage;

public class CreateMessageRequest
{
    [JsonProperty("type")]
    public MessageType Type { get; set; }
    [JsonProperty("clientId")]
    public Guid ClientId { get; set; }
    [JsonProperty("companyId")]
    public Guid CompanyId { get; set; }
    [JsonProperty("content")]
    public string Content { get; set; } = "";
    [JsonProperty("fileName")]
    public string FileName { get; set; } = "";

    [JsonIgnore]
    public Guid CreatedBy { get; set; }
}

