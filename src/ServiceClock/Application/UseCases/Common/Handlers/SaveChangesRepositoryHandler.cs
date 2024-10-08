﻿
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;

namespace ServiceClock_BackEnd.Application.UseCases.Common.Handlers;

public class SaveChangesRepositoryHandler<Domain, Request> : Handler<Request> where Domain : class
{
    private readonly IRepository<Domain> repository;
    public SaveChangesRepositoryHandler
        (ILogService logService,
        IRepository<Domain> repository) 
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

        this.repository.Update(domainObject!);

        sucessor?.ProcessRequest(request);
    }
}

