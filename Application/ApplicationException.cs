
namespace ServiceClock_BackEnd.Application;

public class ApplicationException : Exception
{
    public ApplicationException(string message)
        : base(message)
    {
    }
}

