using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ServiceClock_BackEnd_Api.Filters;

public class PrefixDocumentFilter : IDocumentFilter
{
    private readonly string _prefix;

    public PrefixDocumentFilter(string prefix)
    {
        _prefix = prefix;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var updatedPaths = new OpenApiPaths();

        foreach (var path in swaggerDoc.Paths)
        {
            var newPath = $"/{_prefix}{path.Key}";
            updatedPaths.Add(newPath, path.Value);
        }

        swaggerDoc.Paths = updatedPaths;
    }
}
