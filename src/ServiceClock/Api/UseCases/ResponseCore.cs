using ServiceClock_BackEnd.Api.Helpers.Hateoas;
using ServiceClock_BackEnd.Domain.Models.System;

namespace ServiceClock_BackEnd.Api.UseCases;

public class ResponseCore
{
    public ResponseCore(string scheme)
    {
       _links = HateoasScheme.Instance.GetLinks(scheme);
    }
    public List<Link> _links { get; set; } = new List<Link>();
}

