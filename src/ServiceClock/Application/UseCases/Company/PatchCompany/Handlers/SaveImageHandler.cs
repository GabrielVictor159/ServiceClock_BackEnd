
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Application.UseCases.Client.PatchClient;

namespace ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany.Handlers;

public class SaveImageHandler : Handler<PatchCompanyUseCaseRequest>
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

    public override void ProcessRequest(PatchCompanyUseCaseRequest request)
    {
        if (request.Image != "")
        {
            var result = blobService.SaveBlob(request.Image,request.ImageName);
            if (!result.Sucess)
            {
                this.notificationService.AddNotification("Image not save", $"Não foi possivel salvar a imagem");
                return;
            }

            request.Company.CompanyImage = result.Id;
        }
        sucessor?.ProcessRequest(request);
    }
}


