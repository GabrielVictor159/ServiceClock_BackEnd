
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Application.UseCases.Client.DeleteClient.Handlers;

public class DeleteMessagesHandler : Handler<DeleteClientUseCaseRequest>
{
    private readonly IRepository<Domain.Models.Message> messageRepository;
    private readonly IBlobService blobService;
    public DeleteMessagesHandler
        (ILogService logService,
        IRepository<Domain.Models.Message> messageRepository,
        IBlobService blobService)
        : base(logService)
    {
        this.messageRepository = messageRepository;
        this.blobService = blobService;
    }

    public override void ProcessRequest(DeleteClientUseCaseRequest request)
    {
        foreach (var message in request.Messages)
        {
            if (message.Type != MessageType.Txt)
            {
                blobService.MoveBlobToPrivateContainer(message.MessageContent);
            }
            message.Active = false;
        }
        request.MessagesDeleteRows=this.messageRepository.UpdateRange(request.Messages);
        sucessor?.ProcessRequest(request);
    }
}

