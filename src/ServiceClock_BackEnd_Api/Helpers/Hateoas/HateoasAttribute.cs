using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceClock_BackEnd.Domain.Models.System;

namespace ServiceClock_BackEnd.Helpers.Hateoas;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class HateoasAttribute : Attribute
{
    public string Rel { get; }
    public string HrefTemplate { get; }
    public string Method { get; }

    private static readonly object lockObject = new object(); 

    public HateoasAttribute(string scheme, string rel, string hrefTemplate, string method, Type? requestBody = null)
    {
        Rel = rel;
        HrefTemplate = hrefTemplate;
        Method = method;
        JObject? requestBodyExample = null;
        if (requestBody != null)
        {
            requestBodyExample = GenerateRequestBodyExample(requestBody);
        }
        HateoasScheme.Instance.AddLink(scheme, new Link("/api" + hrefTemplate, rel, method, requestBodyExample));
    }

    private JObject? GenerateRequestBodyExample(Type requestBody)
    {
        lock (lockObject)
        {
            var instance = Activator.CreateInstance(requestBody);
            var jsonObject = JObject.FromObject(instance);

            var propertiesToRemove = jsonObject.Properties()
                .Where(p => p.Value.Type == JTokenType.Null ||
                            p.Value.Type == JTokenType.Undefined ||
                            p.Value.Type == JTokenType.None)
                .ToList();

            foreach (var property in propertiesToRemove)
            {
                property.Remove();
            }

            return jsonObject;
        }
    }
}
