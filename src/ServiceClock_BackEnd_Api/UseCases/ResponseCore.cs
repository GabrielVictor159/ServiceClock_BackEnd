
using ServiceClock_BackEnd.Domain.Models.System;
using ServiceClock_BackEnd.Helpers.Hateoas;

namespace ServiceClock_BackEnd.UseCases;

public class ResponseCore
{
    public ResponseCore(string scheme)
    {
       _links = HateoasScheme.Instance.GetLinks(scheme);
    }
    public List<Link> _links { get; set; } = new List<Link>();
}

