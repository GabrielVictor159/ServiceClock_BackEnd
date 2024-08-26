
using FluentValidation;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Application.UseCases.Common.Handlers;

public class ValidDomainHandler<Domain, Validator, Request> : Handler<Request>
    where Domain : Entity<Domain, Validator>
    where Validator : AbstractValidator<Domain>
{
    private readonly INotificationService notificationService;
    public ValidDomainHandler(INotificationService notificationService, ILogService logService)
        : base(logService)
    {
        this.notificationService = notificationService;
    }
    public override void ProcessRequest(Request request)
    {
        var domainObject = typeof(Request)
                .GetProperties()
                .FirstOrDefault(prop => prop.PropertyType == typeof(Domain))?
                .GetValue(request) as Domain;

        if(domainObject == null)
        {
            throw new ApplicationException($"Could not find any object with the type {typeof(Domain).Name} in the request");
        }

        if(!domainObject.IsValid)
        {
            this.notificationService.AddNotifications(domainObject.ValidationResult);
            return;
        }
        this.sucessor?.ProcessRequest(request);
    }
}

