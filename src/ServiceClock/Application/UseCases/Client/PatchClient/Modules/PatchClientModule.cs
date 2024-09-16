
using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Company.PatchCompany;
using ServiceClock_BackEnd.Domain.Modules;
using ServiceClock_BackEnd.Domain.Validations;
using ServiceClock_BackEnd.Application.UseCases.Client.PatchClient.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Client.CreateClient;

namespace ServiceClock_BackEnd.Application.UseCases.Client.PatchClient.Modules;

public class PatchClientModule : Module
{
    public override void Configure(IServiceCollection services)
    {
        services.AddTransient<IPatchClientUseCase, PatchClientUseCase>();

        services.AddSingleton<SaveChangesRepositoryHandler<Domain.Models.Client, PatchClientUseCaseRequest>>();
        services.AddSingleton<ValidDomainHandler<Domain.Models.Client, ClientValidator, PatchClientUseCaseRequest>>();
        services.AddSingleton<VerifyDisponibilityClientHandler<PatchClientUseCaseRequest>>();
        services.AddSingleton<SearchClientForUpdateHandler>();
        services.AddSingleton<SaveImageHandler>();
    }
}

