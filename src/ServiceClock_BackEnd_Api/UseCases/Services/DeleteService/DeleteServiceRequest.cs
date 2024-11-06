
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ServiceClock_BackEnd.UseCases.Services.DeleteService;

public class DeleteServiceRequest
{
    [JsonProperty("ServiceId")]
    [Required]
    public Guid ServiceId { get; set; }
}

