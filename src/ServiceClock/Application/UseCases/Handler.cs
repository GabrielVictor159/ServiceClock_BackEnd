using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Domain.Helpers;

namespace ServiceClock_BackEnd.Application.UseCases;

public abstract class Handler<T> 
{
    protected Handler<T>? sucessor;
    protected ILogService logService;

    protected Handler(ILogService logService)
    {
        this.logService = logService;
        this.logService.AddClassLog(this.GetType().Name);
    }

    public dynamic SetSucessor(Handler<T> sucessor)
    {
        this.sucessor = sucessor;
        return this;
    }
    public abstract void ProcessRequest(T request);
}

