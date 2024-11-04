
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Domain.Validations;
using ServiceClock_BackEnd.Domain.Helpers;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Application.Boundaries;
using ServiceClock_BackEnd.Application.Boundaries.Company;
using ServiceClock_BackEnd_Application.Interfaces;

namespace ServiceClock_BackEnd.Application.UseCases.Company.CreateCompany;

public class CreateCompanyUseCase : ICreateCompanyUseCase
{
    private readonly IOutputPort<CreateCompanyBoundarie> outputPort;
    private readonly ValidDomainHandler<Domain.Models.Company, CompanyValidator, CreateCompanyUseCaseRequest> validDomainHandler;
    private readonly ILogService logService;

    public CreateCompanyUseCase
        (IOutputPort<CreateCompanyBoundarie> outputPort,
        ILogService logService,
        ValidDomainHandler<Domain.Models.Company, CompanyValidator, CreateCompanyUseCaseRequest> validDomainHandler,
        VerifyDisponibilityCompanyHandler<CreateCompanyUseCaseRequest> verifyDisponibilityCompanyHandler,
        SaveDomainDbHandler<Domain.Models.Company, CreateCompanyUseCaseRequest> saveDomainDbHandler)
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
            validDomainHandler.ProcessRequest(request);
            outputPort.Standard(new() { Company = request.Company });
        }
        catch (Exception ex)
        {
            logService.logs.Add(new(LogType.ERROR, "CreateCompanyUseCase", $"Occurring an error: {ex.Message ?? ex.InnerException?.Message}, stacktrace: {ex.StackTrace}"));
            outputPort.Error(ex.Message!);
        }
    }
}

