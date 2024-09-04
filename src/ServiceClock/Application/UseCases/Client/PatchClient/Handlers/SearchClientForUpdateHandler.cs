
using ServiceClock_BackEnd.Api.UseCases.Client.PatchClient;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;

namespace ServiceClock_BackEnd.Application.UseCases.Client.PatchClient.Handlers;

public class SearchClientForUpdateHandler : Handler<PatchClientUseCaseRequest>
{
    private readonly IRepository<Domain.Models.Client> repository;
    private readonly INotificationService notificationService;
    public SearchClientForUpdateHandler
        (IRepository<Domain.Models.Client> repository,
        ILogService logService,
        INotificationService notificationService)
        : base(logService)
    {
        this.repository = repository;
        this.notificationService = notificationService;
    }

    public override void ProcessRequest(PatchClientUseCaseRequest request)
    {
        var client = repository.GetById(request.Client?.Id.ToString() ?? "");
        if (client == null)
        {
            notificationService.AddNotification("Client not found", "Não foi encontrado nenhum cliente com esse Id");
            return;
        }
        client.Name = request.Client!.Name != "" ? request.Client.Name : client.Name;
        client.Password = request.Client!.Password != "" ? request.Client.Password : client.Password;
        client.Address = request.Client!.Address != "" ? request.Client.Address : client.Address;
        client.City = request.Client!.City != "" ? request.Client.City : client.City;
        client.State = request.Client!.State != "" ? request.Client.State : client.State;
        client.Country = request.Client!.Country != "" ? request.Client.Country : client.Country;
        client.PostalCode = request.Client!.PostalCode != "" ? request.Client.PostalCode : client.PostalCode;
        client.PhoneNumber = request.Client!.PhoneNumber != "" ? request.Client.PhoneNumber : client.PhoneNumber;
        client.BirthDate = request.Client!.BirthDate != DateTime.MinValue ? request.Client.BirthDate : client.BirthDate;

        request.Client = client;
        sucessor?.ProcessRequest(request);
    }
}

