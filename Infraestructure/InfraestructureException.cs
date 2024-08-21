
namespace ServiceClock_BackEnd.Infraestructure;

public class InfraestructureException : Exception
{
    public InfraestructureException(string message)
        : base(message)
    {
    }
}

