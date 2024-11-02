
using ServiceClock_BackEnd.Api.UseCases.Client.DeleteClient;
using ServiceClock_BackEnd.Api.UseCases.Client.PatchClient;
using ServiceClock_BackEnd.Application.Boundaries;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Application.UseCases.Client.DeleteClient.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Client.PatchClient.Handlers;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Infraestructure.Services;

namespace ServiceClock_BackEnd.Application.UseCases.Client.DeleteClient;

public class DeleteClientUseCase : IDeleteClientUseCase
{
    private readonly ILogService logService;
    private readonly DeleteClientPresenter outputPort;
    private readonly GetAppointmentsAndMessagesHandler handler;

    public DeleteClientUseCase
        (ILogService logService, 
        DeleteClientPresenter outputPort, 
        GetAppointmentsAndMessagesHandler handler,
        DeleteMessagesHandler deleteMessagesHandler,
        DeleteAppointmentsHandler deleteAppointmentsHandler,
        DeleteClientHandler deleteClientHandler)
    {
        handler
            .SetSucessor(deleteMessagesHandler
            .SetSucessor(deleteAppointmentsHandler
            .SetSucessor(deleteClientHandler)));

        this.logService = logService;
        this.outputPort = outputPort;
        this.handler = handler;
    }

    public void Execute(DeleteClientUseCaseRequest request)
    {
        try
        {
            handler.ProcessRequest(request);

            outputPort.Standard(new() { 
                Appointments = request.Appointments, 
                Messages = request.Messages, 
                AppointmentsDeleteRows = request.AppointmentsDeleteRows,
                MessagesDeleteRows = request.MessagesDeleteRows,
                ClientDeleteRows = request.ClientDeleteRows});
        }
        catch (Exception ex)
        {
            logService.logs.Add(new(LogType.ERROR, "PatchClientUseCase", $"Occurring an error: {ex.Message ?? ex.InnerException?.Message}, stacktrace: {ex.StackTrace}"));
            outputPort.Error(ex.Message!);
        }
    }
}

