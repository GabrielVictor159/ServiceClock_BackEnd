
using ServiceClock_BackEnd.Api.UseCases.Services.CreateService;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Domain.Validations;

namespace ServiceClock_BackEnd.Application.UseCases.Services.CreateService;

public class CreateServiceUseCase : ICreateServiceUseCase
{
    private readonly CreateServicePresenter outputPort;
    private readonly ILogService logService;
    private readonly ValidDomainHandler<Domain.Models.Service, ServiceValidator, CreateServiceUseCaseRequest> validDomainHandler;

    public CreateServiceUseCase
        (CreateServicePresenter outputPort, 
        ILogService logService, 
        ValidDomainHandler<Service, ServiceValidator, CreateServiceUseCaseRequest> validDomainHandler,
        VerifyDisponibilityServiceHandler<CreateServiceUseCaseRequest> verifyDisponibilityServiceHandler,
        SaveDomainDbHandler<Domain.Models.Service, CreateServiceUseCaseRequest> saveDomainDbHandler)
    {
        validDomainHandler
            .SetSucessor(verifyDisponibilityServiceHandler
            .SetSucessor(saveDomainDbHandler));

        this.outputPort = outputPort;
        this.logService = logService;
        this.validDomainHandler = validDomainHandler;
    }

    public void Execute(CreateServiceUseCaseRequest request)
    {
        try
        {
            validDomainHandler.ProcessRequest(request);
            outputPort.Standard(new() { Service = request.Service });
        }
        catch (Exception ex)
        {
            logService.logs.Add(new(LogType.ERROR, "CreateCompanyUseCase", $"Occurring an error: {ex.Message ?? ex.InnerException?.Message}, stacktrace: {ex.StackTrace}"));
            outputPort.Error(ex.Message!);
        }
    }
}

