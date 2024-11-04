
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Application.Interfaces.Services;

public interface ILogService
{
    public List<Log> logs { get; set; }
    public void PopulateLogs();
    void AddClassLog(string className);
}

