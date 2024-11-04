
using ServiceClock_BackEnd.Domain.Models.System;

namespace ServiceClock_BackEnd.Helpers.Hateoas;

using System.Collections.Concurrent;

public class HateoasScheme
{
    private static readonly Lazy<HateoasScheme> _instance = new Lazy<HateoasScheme>(() => new HateoasScheme());

    private readonly ConcurrentDictionary<string, List<Link>> _schemas = new ConcurrentDictionary<string, List<Link>>();

    private HateoasScheme() { }

    public static HateoasScheme Instance => _instance.Value;

    public void AddLink(string methodName, Link link)
    {
        _schemas.AddOrUpdate(methodName,
            new List<Link> { link },
            (key, existingList) =>
            {
                existingList.Add(link);
                return existingList;
            });
    }

    public List<Link> GetLinks(string methodName)
    {
        return _schemas.ContainsKey(methodName) ? _schemas[methodName] : new List<Link>();
    }
}



