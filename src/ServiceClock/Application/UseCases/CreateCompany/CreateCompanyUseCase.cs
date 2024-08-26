
using ServiceClock_BackEnd.Api.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.Application.Boundaries;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Domain.Validations;
using ServiceClock_BackEnd.Domain.Helpers;
using ServiceClock_BackEnd.Application.Interfaces.Services;

namespace ServiceClock_BackEnd.Application.UseCases.CreateCompany;

public class CreateCompanyUseCase : ICreateCompanyUseCase
{
    private readonly CreateCompanyPresenter outputPort;
    private readonly ValidDomainHandler<Company, CompanyValidator, CreateCompanyUseCaseRequest> validDomainHandler;
    private readonly ILogService logService;

    public CreateCompanyUseCase
        (CreateCompanyPresenter outputPort,
        ILogService logService,
        ValidDomainHandler<Company, CompanyValidator, CreateCompanyUseCaseRequest> validDomainHandler,
        VerifyDisponibilityCompanyHandler<CreateCompanyUseCaseRequest> verifyDisponibilityCompanyHandler,
        SaveDomainDbHandler<Company, CreateCompanyUseCaseRequest> saveDomainDbHandler)
    {
        validDomainHandler
            .SetSucessor(verifyDisponibilityCompanyHandler
            .SetSucessor(saveDomainDbHandler));

        this.outputPort = outputPort;
        this.validDomainHandler = validDomainHandler;
        this.logService = logService;
        this.logService.AddClassLog("CreateCompanyUseCase");
    }

    public void Execute(CreateCompanyUseCaseRequest request)
    {
        try
        {
            this.validDomainHandler.ProcessRequest(request);
            outputPort.Standard(new() { Company = request.Company});
        }
        catch (Exception ex)
        {
            this.logService.logs.Add(new(LogType.ERROR, "CreateCompanyUseCase", $"Occurring an error: {ex.Message ?? ex.InnerException?.Message}, stacktrace: {ex.StackTrace}"));
            outputPort.Error(ex.Message!);
        }
    }
}

