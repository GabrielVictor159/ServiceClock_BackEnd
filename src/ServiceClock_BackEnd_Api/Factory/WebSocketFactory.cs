using Autofac;
using Newtonsoft.Json;
using ServiceClock_BackEnd_Api.Factory.Handlers;
using System.Net.WebSockets;
using System.Text;
using System.Reflection;
using Azure.Core;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd_Api.UseCases.Messages.ListMessage;
using ServiceClock_BackEnd.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ServiceClock_BackEnd_Api.Factory;

public class WebSocketFactory
{
    private readonly ILifetimeScope _scope;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public WebSocketFactory(ILifetimeScope scope, IHttpContextAccessor httpContextAccessor, TokenValidationParameters tokenValidationParameters)
    {
        _scope = scope;
        _httpContextAccessor = httpContextAccessor;
        _tokenValidationParameters = tokenValidationParameters;
    }

    public async Task Echo(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!result.CloseStatus.HasValue)
        {
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Console.WriteLine($"Mensagem recebida: {message}");

            var body = JsonConvert.DeserializeObject<WebSocketBody>(message);
            if (body != null)
            {
                var handlerType = GetTypeByPartialName(body.target);
                var context = _httpContextAccessor.HttpContext;
                if (handlerType != null)
                {
                    var hasAllowAnonymous = handlerType.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).Any();

                    if (!TryAuthenticateWebSocketUser(body.Authorization, context) && !hasAllowAnonymous)
                    {
                        await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("Você não esta autenticado")),
                                                  WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    else
                    {
                        try
                        {
                            var handler = _scope.Resolve(handlerType) as dynamic;
                            if (handler != null)
                            {
                                handler.Context = context;
                                handler.WebSocket = webSocket;
                                await handler.Handler(body.arguments);
                            }
                            else
                            {
                                Console.WriteLine("Handler não encontrado.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Occurring an error: {ex.Message ?? ex.InnerException?.Message}, stacktrace: {ex.StackTrace}");
                            await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("Ocorreu um erro na execução do socket.")),
                                                      WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Tipo de handler não encontrado.");
                    await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("Tipo de handler não encontrado.")),
                                              WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }

            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }

    private Type? GetTypeByPartialName(string partialName)
    {
        var type = Assembly.GetExecutingAssembly().GetTypes()
            .FirstOrDefault(t => t.Name.Equals(partialName, StringComparison.OrdinalIgnoreCase));

        if (type != null)
        {
            var webSocketHandlerType = typeof(WebSocketHandler<>);

            var currentType = type;
            while (currentType != null)
            {
                if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == webSocketHandlerType)
                {
                    return type; 
                }
                currentType = currentType.BaseType;
            }
        }

        return null; 
    }

    private bool TryAuthenticateWebSocketUser(string? token, HttpContext? context)
    {
        if (string.IsNullOrEmpty(token) || context == null)
            return false;

        if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            token = token.Substring("Bearer ".Length);
        }

        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwtToken || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            context.User = principal;
            return true;
        }
        catch
        {
            return false;
        }
    }


    private class WebSocketBody
    {
        public string target { get; set; } = "";
        public dynamic? arguments { get; set; }
        public string Authorization { get; set; } = "";
    }
}
