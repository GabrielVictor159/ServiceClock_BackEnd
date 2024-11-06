using Microsoft.Extensions.Logging;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Models;
using System.Reflection;
using System.Runtime.Serialization;

namespace ServiceClock_BackEnd.Infraestructure.Services;

public class LogService : ILogService
{
    public List<Log> logs { get; set; } = new List<Log>();
    private readonly IRepository<Log> repository;
    private readonly ILogger<LogService> logger;

    public LogService(IRepository<Log> repository, ILogger<LogService> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }

    public void AddClassLog(string className)
    {
        logs.Add(new Log(LogType.PROCESS, className, "The flow arrived in this class"));
    }

    public void PopulateLogs()
    {
        string errorEnumMemberValue = GetEnumMemberValue(LogType.ERROR);
        foreach (var log in logs)
        {
            if (log.Type == errorEnumMemberValue)
            {
                this.repository.Add(log);
            }
            else
            {
                logger.LogInformation(
                $"Id: {log.Id} - {log.Message}" +
                $"[{log.LogDate:yyyy-MM-dd HH:mm:ss}] " +
                $"[{log.Type}] " +
                $"[{log.Class}] ");
            }
        }

        this.logs.Clear();
    }

    private string GetEnumMemberValue(LogType logType)
    {
        var fieldInfo = typeof(LogType).GetField(logType.ToString());

        if (fieldInfo == null)
        {
            throw new InvalidOperationException($"Field '{logType}' not found in enum '{nameof(LogType)}'.");
        }

        var attribute = fieldInfo.GetCustomAttribute(typeof(EnumMemberAttribute), false) as EnumMemberAttribute;

        return attribute?.Value ?? string.Empty;
    }


}
