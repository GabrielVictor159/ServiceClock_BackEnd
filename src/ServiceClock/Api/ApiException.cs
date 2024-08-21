
namespace ServiceClock_BackEnd.Api;

public class ApiException : Exception
{
    public ApiException(string message)
        : base(message)
    {
    }
}

