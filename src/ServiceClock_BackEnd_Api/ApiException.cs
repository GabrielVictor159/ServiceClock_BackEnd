
namespace ServiceClock_BackEnd.Infraestructure;

public class ApiException : Exception
{
    public ApiException(string message)
        : base(message)
    {
    }
}

