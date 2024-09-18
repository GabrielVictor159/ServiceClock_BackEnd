
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Domain.Enums;

namespace ServiceClock_BackEnd.Application.UseCases.Messages.CreateMessage.Handlers;

public class SaveFileHandler : Handler<CreateMessageUseCaseRequest>
{
    private readonly IBlobService blobService;
    private readonly INotificationService notificationService;
    public SaveFileHandler
        (ILogService logService,
        IBlobService blobService,
        INotificationService notificationService)
        : base(logService)
    {
        this.blobService = blobService;
        this.notificationService = notificationService;
    }

    public override void ProcessRequest(CreateMessageUseCaseRequest request)
    {
        if((int)request.Message.Type >(int)MessageType.Txt)
        {
            var result = blobService.SaveBlob(request.Content, request.FileName);
            if(!result.Sucess)
            {
                notificationService.AddNotification("Not complete save file",result.e !=null ? result.e.Message : "Não foi possivel completar o processo de salvamento do arquivo");
                return;
            }

            request.Message.MessageContent = result.Id;
        }

        sucessor?.ProcessRequest(request);
    }
}

