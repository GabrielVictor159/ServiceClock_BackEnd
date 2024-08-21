
namespace ServiceClock_BackEnd.Application.Interfaces.Services;

public interface ITokenService
{
    public string Generate(string rule, Guid IdUser);
}

