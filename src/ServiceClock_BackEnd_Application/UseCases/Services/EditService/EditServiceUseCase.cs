
using ServiceClock_BackEnd.Application.Boundaries.Services;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Services.DeleteService.Handlers;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Domain.Validations;
using ServiceClock_BackEnd_Application.Interfaces;

namespace ServiceClock_BackEnd.Application.UseCases.Services.EditService;

public class EditServiceUseCase : IEditServiceUseCase
{
    private readonly IOutputPort<EditServiceBoundarie> outputPort;
    private readonly ILogService logService;
    private readonly ValidDomainHandler<Service, ServiceValidator, EditServiceUseCaseRequest> handler;

    public EditServiceUseCase
        (IOutputPort<EditServiceBoundarie> outputPort,
        ILogService logService,
        ValidDomainHandler<Service, ServiceValidator, EditServiceUseCaseRequest> handler,
        SaveChangesRepositoryHandler<Service, EditServiceUseCaseRequest> saveChangesRepositoryHandler)
    {
        handler
            .SetSucessor(saveChangesRepositoryHandler);

        this.outputPort = outputPort;
        this.logService = logService;
        this.handler = handler;
    }
    public void Execute(EditServiceUseCaseRequest request)
    {
        try
        {
            this.handler.ProcessRequest(request);
            outputPort.Standard(new() { Service = request.service });
        }
        catch (Exception ex)
        {
            logService.logs.Add(new(LogType.ERROR, "CreateCompanyUseCase", $"Occurring an error: {ex.Message ?? ex.InnerException?.Message}, stacktrace: {ex.StackTrace}"));
            outputPort.Error(ex.Message!);
        }
    }
}

