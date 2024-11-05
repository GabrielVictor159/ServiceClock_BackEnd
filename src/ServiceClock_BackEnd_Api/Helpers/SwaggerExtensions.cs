using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ServiceClock_BackEnd_Api.Helpers;

public static class SwaggerExtensions
{
    public static void AddSignalRDocumentation(this SwaggerGenOptions options)
    {
        options.OperationFilter<SignalROperationFilter>();
    }
}

public class SignalROperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.RelativePath != null && context.ApiDescription.RelativePath.Contains("listMessageHub"))
        {
            operation.Summary = "Métodos disponíveis no ListMessageHub.";
            operation.Description = "Este hub permite enviar e receber mensagens.";

            operation.Responses.Add("200", new OpenApiResponse { Description = "Mensagem recebida com sucesso." });
            operation.Responses.Add("400", new OpenApiResponse { Description = "Erro ao enviar a mensagem." });
        }
    }
}

