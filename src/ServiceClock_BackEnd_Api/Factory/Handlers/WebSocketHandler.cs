using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace ServiceClock_BackEnd_Api.Factory.Handlers;

public abstract class WebSocketHandler<T>
{
    public WebSocket? WebSocket { get; set; }
    public HttpContext? Context { get; set; }
    public abstract Task Process(T request);

    public async Task Handler(dynamic request)
    {
        var requestString = JsonConvert.SerializeObject(request);
        try
        {
            var requestConvert = JsonConvert.DeserializeObject<T>(requestString);
            if (requestConvert == null)
            {
                Console.WriteLine("Tipo de argumento não corresponde.");
                if (WebSocket != null)
                {
                    await WebSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("Tipo de argumento incorreto.")),
                                              WebSocketMessageType.Text, true, CancellationToken.None);
                }
                return;
            }
            else
            {
                await this.Process(requestConvert);
            }
        }
        catch
        {
            Console.WriteLine("Tipo de argumento não corresponde.");
            if (WebSocket != null)
            {
                await WebSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("Tipo de argumento incorreto.")),
                                          WebSocketMessageType.Text, true, CancellationToken.None);
            }
            return;
        }
    }

}

