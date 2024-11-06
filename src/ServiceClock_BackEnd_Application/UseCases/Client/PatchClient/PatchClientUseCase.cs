
using ServiceClock_BackEnd.Application.Boundaries;
using ServiceClock_BackEnd.Application.Boundaries.Client;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Application.UseCases.Client.CreateClient;
using ServiceClock_BackEnd.Application.UseCases.Client.PatchClient.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany;
using ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany.Handlers;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Validations;
using ServiceClock_BackEnd_Application.Interfaces;

namespace ServiceClock_BackEnd.Application.UseCases.Client.PatchClient;

public class PatchClientUseCase : IPatchClientUseCase
{
    private readonly ILogService logService;
    private readonly IOutputPort<PatchClientBoundarie> outputPort;
    private readonly SearchClientForUpdateHandler handler;

    public PatchClientUseCase
        (ILogService logService,
        IOutputPort<PatchClientBoundarie> outputPort,
        SearchClientForUpdateHandler searchClientForUpdateHandler,
        ValidDomainHandler<Domain.Models.Client, ClientValidator, PatchClientUseCaseRequest> validDomainHandler,
        VerifyDisponibilityClientHandler<PatchClientUseCaseRequest> verifyDisponibilityCompanyHandler,
        SaveChangesRepositoryHandler<Domain.Models.Client, PatchClientUseCaseRequest> saveChangesHandler,
        Handlers.SaveImageHandler saveImageHandler)
    {
        searchClientForUpdateHandler
            .SetSucessor(validDomainHandler
            .SetSucessor(verifyDisponibilityCompanyHandler
            .SetSucessor(saveImageHandler
            .SetSucessor(saveChangesHandler))));

        this.handler = searchClientForUpdateHandler;
        this.logService = logService;
        this.outputPort = outputPort;
    }

    public void Execute(PatchClientUseCaseRequest request)
    {
        try
        {
            handler.ProcessRequest(request);

            outputPort.Standard(new() { Client = request.Client });
        }
        catch (Exception ex)
        {
            logService.logs.Add(new(LogType.ERROR, "PatchClientUseCase", $"Occurring an error: {ex.Message ?? ex.InnerException?.Message}, stacktrace: {ex.StackTrace}"));
            outputPort.Error(ex.Message!);
        }
    }
}

