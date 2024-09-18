
using ServiceClock_BackEnd.Api.UseCases.Messages.CreateMessage;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Messages.CreateMessage.Handlers;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Domain.Validations;
using ServiceClock_BackEnd.Infraestructure.Services;

namespace ServiceClock_BackEnd.Application.UseCases.Messages.CreateMessage;

public class CreateMessageUseCase : ICreateMessageUseCase
{
    private readonly CreateMessagePresenter outputPort;
    private readonly ILogService logService;
    private readonly ValidDomainHandler<Message, MessageValidator, CreateMessageUseCaseRequest> handler;

    public CreateMessageUseCase
        (CreateMessagePresenter outputPort,
        ILogService logService, 
        ValidDomainHandler<Message, MessageValidator, CreateMessageUseCaseRequest> validDomainHandler,
        SaveFileHandler saveFileHandler,
        SaveDomainDbHandler<Message, CreateMessageUseCaseRequest> saveDomainDbHandler)
    {
        validDomainHandler
            .SetSucessor(saveFileHandler
            .SetSucessor(saveDomainDbHandler));

        this.outputPort = outputPort;
        this.logService = logService;
        this.handler = validDomainHandler;
    }

    public void Execute(CreateMessageUseCaseRequest request)
    {
        try
        {
            handler.ProcessRequest(request);

            outputPort.Standard(new() { Message = request.Message });
        }
        catch (Exception ex)
        {
            logService.logs.Add(new(LogType.ERROR, "PatchCompanyUseCase", $"Occurring an error: {ex.Message ?? ex.InnerException?.Message}, stacktrace: {ex.StackTrace}"));
            outputPort.Error(ex.Message!);
        }
    }
}

