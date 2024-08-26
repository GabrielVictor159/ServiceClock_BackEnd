
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Infraestructure.Services;

public class LogService : ILogService
{
    public List<Log> logs { get; set; } = new List<Log>();
    private readonly IRepository<Log> repository;

    public LogService(IRepository<Log> repository)
    {
        this.repository = repository;
    }
    public void AddClassLog(string className)
    {
        logs.Add(new Log(LogType.PROCESS, className, "The flow arrived in this class"));
    }
    public void PopulateLogs()
    {
        this.repository.AddRange(logs);
        this.logs.Clear();
    }
}

