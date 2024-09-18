
using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany;
using ServiceClock_BackEnd.Domain.Modules;
using ServiceClock_BackEnd.Domain.Validations;
using ServiceClock_BackEnd.Application.UseCases.Messages.CreateMessage.Handlers;

namespace ServiceClock_BackEnd.Application.UseCases.Messages.CreateMessage.Modules;

public class CreateMessageModule : Module
{
    public override void Configure(IServiceCollection services)
    {
        services.AddTransient<ICreateMessageUseCase, CreateMessageUseCase>();

        services.AddSingleton<ValidDomainHandler<Domain.Models.Message, MessageValidator, CreateMessageUseCaseRequest>>();
        services.AddSingleton<SaveFileHandler>();
        services.AddSingleton<SaveDomainDbHandler<Domain.Models.Message, CreateMessageUseCaseRequest>>();
    }
}

