
using ServiceClock_BackEnd.Domain.Enums;

namespace ServiceClock_BackEnd.Domain.Models;

public class Log
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Type { get; set; }
    public string Class { get; set; } = "";
    public string Message { get; set; } = "";
    public DateTime LogDate { get; set; } = DateTime.Now;
    public Log( LogType type, string clas, string message)
    {
        Type = type.ToString();
        Class = clas;
        Message = message;
    }
    public Log()
    { }
}

