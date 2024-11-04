
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;

namespace ServiceClock_BackEnd.Application.UseCases.Common.Handlers;

public class SaveDomainDbHandler<Domain, Request> : Handler<Request> where Domain : class
{
    private readonly IRepository<Domain> repository;
    public SaveDomainDbHandler(IRepository<Domain> repository, ILogService logService)
        : base(logService)
    {
        this.repository = repository;
    }
    public override void ProcessRequest(Request request)
    {
        var domainObject = typeof(Request)
                .GetProperties()
                .FirstOrDefault(prop => prop.PropertyType == typeof(Domain))?
                .GetValue(request) as Domain;

        if (domainObject == null)
        {
            throw new ApplicationException($"Could not find any object with the type {typeof(Domain).Name} in the request");
        }

        repository.Add(domainObject);
        this.sucessor?.ProcessRequest(request);
    }
}


