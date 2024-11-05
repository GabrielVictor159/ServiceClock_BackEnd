using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Infraestructure.Data;
using ServiceClock_BackEnd.UseCases.Messages.ListMessage;

namespace ServiceClock_BackEnd_Api.UseCases.Messages.ListMessage;
[Authorize]
public class ListMessageHub : Hub
{
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Message> _messageRepository;
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Client> _clientRepository;

    public ListMessageHub
        (IRepository<ServiceClock_BackEnd.Domain.Models.Message> messageRepository, 
        IRepository<ServiceClock_BackEnd.Domain.Models.Client> clientRepository)
    {
        _messageRepository = messageRepository;
        _clientRepository = clientRepository;
    }

    public async Task SendMessage(ListMessageRequest request)
    {
        var userId = Context.User!.FindFirst("User_Id")?.Value;
        var userType = Context.User!.FindFirst("User_Rule")?.Value;

        if (userType == "Client")
        {
            request.ClientId = Guid.Parse(userId!);
        }
        else if (userType == "Company")
        {
            request.CompanyId = Guid.Parse(userId!);
        }

        var client = _clientRepository.Find(e => e.Id == request.ClientId && e.Active);
        if (client == null)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "Cliente não encontrado");
            return;
        }

        var messages = _messageRepository
            .Find(e => e.ClientId == request.ClientId && e.CompanyId == request.CompanyId && e.Active);

        await Clients.Caller.SendAsync("ReceiveMessages", messages);
    }
}
