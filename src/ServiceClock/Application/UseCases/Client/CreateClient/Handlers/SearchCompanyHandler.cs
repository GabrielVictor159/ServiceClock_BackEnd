
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;

namespace ServiceClock_BackEnd.Application.UseCases.Client.CreateClient.Handlers;
#pragma warning disable CS8604
public class SearchCompanyHandler : Handler<CreateClientUseCaseRequest>
{
    private readonly IRepository<Domain.Models.Company> repository;
    private readonly INotificationService notificationService;

    public SearchCompanyHandler
        (ILogService logService,
        IRepository<Domain.Models.Company> repository,
        INotificationService notificationService) 
        : base(logService)
    {
        this.repository = repository;
        this.notificationService = notificationService;
    }

    public override void ProcessRequest(CreateClientUseCaseRequest request)
    {
        var companyId = request.Client?.CompanyId;
        if (companyId != null)
        {
            var company = this.repository.Find(e=>e.Id==companyId).FirstOrDefault();
            if (company == null)
            {
                this.notificationService.AddNotification("Company not found","Não foi possivel encontrar o id da empresa");
                return;
            }
        }
        else
        {
            this.notificationService.AddNotification("Company not found", "Por favor forneça o CompanyId no Client");
            return;
        }
        sucessor?.ProcessRequest(request);
    }
}
#pragma warning restore CS8604

