﻿
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Domain.Enums;

namespace ServiceClock_BackEnd.Application.UseCases.Client.PatchClient.Handlers;

public class SaveImageHandler : Handler<PatchClientUseCaseRequest>
{
    private readonly IBlobService blobService;
    private readonly INotificationService notificationService;

    public SaveImageHandler
        (ILogService logService,
        IBlobService blobService,
        INotificationService notificationService) 
        : base(logService)
    {
        this.blobService = blobService;
        this.notificationService = notificationService;
    }

    public override void ProcessRequest(PatchClientUseCaseRequest request)
    {
        if (request.Image != "")
        {
            var result = blobService.SaveImage(request.Image);
            if (!result.Sucess)
            {
                this.notificationService.AddNotification("Image not save", $"Não foi possivel salvar a imagem");
                return;
            }

            request.Client.ClientImage = result.Id;
        }
        sucessor?.ProcessRequest(request);
    }
}

