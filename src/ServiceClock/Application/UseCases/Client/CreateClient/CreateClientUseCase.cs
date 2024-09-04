using ServiceClock_BackEnd.Api.UseCases.Client.CreateClient;
using ServiceClock_BackEnd.Application.Boundaries;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Application.UseCases.Client.CreateClient.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Application.UseCases.Client.CreateClient;
public class CreateClientUseCase : ICreateClientUseCase
{
    private readonly ILogService logService;
    private readonly CreateClientPresenter outputPort;
    private readonly ValidDomainHandler<Domain.Models.Client, ClientValidator, CreateClientUseCaseRequest> validDomainHandler;
    public CreateClientUseCase
        (ILogService logService, 
        CreateClientPresenter outputPort,
        ValidDomainHandler<Domain.Models.Client, ClientValidator, CreateClientUseCaseRequest> validDomainHandler,
        VerifyDisponibilityClientHandler<CreateClientUseCaseRequest> verifyDisponibilityClientHandler,
        SaveDomainDbHandler<Domain.Models.Client, CreateClientUseCaseRequest> saveDomainDbHandler,
        SearchCompanyHandler searchCompanyHandler)
    {
        validDomainHandler
            .SetSucessor(searchCompanyHandler
            .SetSucessor(verifyDisponibilityClientHandler
            .SetSucessor(saveDomainDbHandler)));

        this.validDomainHandler = validDomainHandler;
        this.logService = logService;
        this.outputPort = outputPort;
    }
    public void Execute(CreateClientUseCaseRequest request)
    {
        try
        {
            this.validDomainHandler.ProcessRequest(request);
            outputPort.Standard(new() { Client = request.Client });
        }
        catch (Exception ex)
        {
            logService.logs.Add(new(LogType.ERROR, "CreateClientUseCase", $"Occurring an error: {ex.Message ?? ex.InnerException?.Message}, stacktrace: {ex.StackTrace}"));
            outputPort.Error(ex.Message!);
        }
    }
}
