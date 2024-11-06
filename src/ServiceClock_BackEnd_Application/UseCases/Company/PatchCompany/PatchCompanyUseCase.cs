
using ServiceClock_BackEnd.Application.Boundaries;
using ServiceClock_BackEnd.Application.Boundaries.Company;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany.Handlers;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Domain.Validations;
using ServiceClock_BackEnd_Application.Interfaces;

namespace ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany;

public class PatchCompanyUseCase : IPatchCompanyUseCase
{
    private readonly INotificationService notificationService;
    private readonly IOutputPort<PatchCompanyBoundarie> outputPort;
    private readonly ILogService logService;
    private readonly SearchCompanyForUpdateHandler handler;

    public PatchCompanyUseCase
        (SearchCompanyForUpdateHandler searchCompanyForUpdateHandler,
        ValidDomainHandler<Domain.Models.Company, CompanyValidator, PatchCompanyUseCaseRequest> validDomainHandler,
        VerifyDisponibilityCompanyHandler<PatchCompanyUseCaseRequest> verifyDisponibilityCompanyHandler,
        SaveChangesRepositoryHandler<Domain.Models.Company, PatchCompanyUseCaseRequest> saveChangesHandler,
        SaveImageHandler saveImageHandler,
        ILogService logService,
        INotificationService notificationService,
        IOutputPort<PatchCompanyBoundarie> outputPort)
    {
        searchCompanyForUpdateHandler
            .SetSucessor(validDomainHandler
            .SetSucessor(verifyDisponibilityCompanyHandler
            .SetSucessor(saveImageHandler
            .SetSucessor(saveChangesHandler))));

        this.handler = searchCompanyForUpdateHandler;
        this.notificationService = notificationService;
        this.outputPort = outputPort;
        this.logService = logService;
    }

    public void Execute(PatchCompanyUseCaseRequest request)
    {
        try
        {
            handler.ProcessRequest(request);

            outputPort.Standard(new() { Company = request.Company });
        }
        catch (Exception ex)
        {
            logService.logs.Add(new(LogType.ERROR, "PatchCompanyUseCase", $"Occurring an error: {ex.Message ?? ex.InnerException?.Message}, stacktrace: {ex.StackTrace}"));
            outputPort.Error(ex.Message!);
        }
    }
}

