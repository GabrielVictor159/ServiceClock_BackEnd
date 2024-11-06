
namespace ServiceClock_BackEnd.Application.UseCases;

public interface IUseCase<T>
{
    void Execute(T request);
}

