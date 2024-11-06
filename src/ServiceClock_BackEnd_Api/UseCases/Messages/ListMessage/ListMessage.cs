
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Infraestructure.Data;
using ServiceClock_BackEnd.UseCases.Messages.ListMessage;
using ServiceClock_BackEnd_Api.Factory.Handlers;
using System.Net.WebSockets;
using System.Text;

namespace ServiceClock_BackEnd_Api.UseCases.Messages.ListMessage;

public class ListMessage : WebSocketHandler<ListMessageRequest>
{
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Client> clientRepository;
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Message> messageRepository;

    public ListMessage
        (IRepository<ServiceClock_BackEnd.Domain.Models.Client> clientRepository, 
        IRepository<Message> messageRepository)
    {
        this.clientRepository = clientRepository;
        this.messageRepository = messageRepository;
    }

    public override async Task Process(ListMessageRequest request)
    {
        var userId = Context!.User!.FindFirst("User_Id")?.Value;
        var userType = Context!.User!.FindFirst("User_Rule")?.Value;

        if (userType == "Client")
        {
            request.ClientId = Guid.Parse(userId!);
        }
        else if (userType == "Company")
        {
            request.CompanyId = Guid.Parse(userId!);
        }

        var client = clientRepository.Find(e => e.Id == request.ClientId && e.Active).FirstOrDefault();
        if (client == null)
        {
            await WebSocket!.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("Cliente não encontrado")),
                                              WebSocketMessageType.Text, true, CancellationToken.None);
            return;
        }

        var messages = messageRepository
            .Find(e => e.ClientId == request.ClientId && e.CompanyId == request.CompanyId && (request.MinDate==null?true: e.CreateAt> request.MinDate) && e.Active )
            .Select(e=> 
            new {Id = e.Id, Type = e.Type, ClientId = e.ClientId, CompanyId = e.CompanyId, CreatedBy = e.CreatedBy, 
                MessageContent = e.MessageContent, CreateAt = e.CreateAt})
            .ToList().OrderBy(e=>e.CreateAt);

        var jsonMessages = JsonConvert.SerializeObject(messages);

        await WebSocket!.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonMessages)),
                                  WebSocketMessageType.Text, true, CancellationToken.None);
    }
}
