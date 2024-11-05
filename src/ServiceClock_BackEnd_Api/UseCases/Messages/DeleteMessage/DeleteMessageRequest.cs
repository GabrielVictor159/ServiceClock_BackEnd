
using Newtonsoft.Json;

namespace ServiceClock_BackEnd.UseCases.Messages.DeleteMessage;

public class DeleteMessageRequest
{
    [JsonProperty("MessageId")]
    public Guid MessageId { get; set; }
}

