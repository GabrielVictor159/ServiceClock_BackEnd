
using Microsoft.Extensions.DependencyInjection;

namespace ServiceClock_BackEnd.Domain.Modules;

public abstract class Module
{
    public abstract void Configure(IServiceCollection services);
}

