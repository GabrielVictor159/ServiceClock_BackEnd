
using Newtonsoft.Json.Linq;

namespace ServiceClock_BackEnd.Domain.Models.System;

public class Link
{
    public Link(string href, string rel, string method, JObject? requestBody)
    {
        Href = href;
        Rel = rel;
        Method = method;
        RequestBody = requestBody;
    }

    public string Href { get; set; }
    public string Rel { get; set; }
    public string Method { get; set; }
    public JObject? RequestBody { get; set; }
}


