using Microsoft.Extensions.DependencyInjection;
using ServiceClock_BackEnd.Application.UseCases.Client.CreateClient.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Common.Handlers;
using ServiceClock_BackEnd.Application.UseCases.Company.CreateCompany;
using ServiceClock_BackEnd.Domain.Modules;
using ServiceClock_BackEnd.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Application.UseCases.Client.CreateClient.Modules;
public class CreateClientModule : Module
{
    public override void Configure(IServiceCollection services)
    {
        services.AddTransient<ICreateClientUseCase, CreateClientUseCase>();

        services.AddSingleton<ValidDomainHandler<Domain.Models.Client, ClientValidator, CreateClientUseCaseRequest>>();
        services.AddSingleton<VerifyDisponibilityClientHandler<CreateClientUseCaseRequest>>();
        services.AddSingleton<SaveDomainDbHandler<Domain.Models.Client, CreateClientUseCaseRequest>>();
        services.AddSingleton<SearchCompanyHandler>();
    }
}
