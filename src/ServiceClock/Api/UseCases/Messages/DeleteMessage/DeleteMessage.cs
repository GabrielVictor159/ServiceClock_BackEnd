
using ServiceClock_BackEnd.Api.Filters;
using ServiceClock_BackEnd.Api.Validator.Http;

namespace ServiceClock_BackEnd.Api.UseCases.Messages.DeleteMessage;

public class DeleteMessage : UseCaseCore
{
    public DeleteMessage
        (HttpRequestValidator httpRequestValidator, 
        NotificationMiddleware middleware) 
        : base(httpRequestValidator.AddValidator(new AuthorizationValidator()), middleware)
    {
    }
}

