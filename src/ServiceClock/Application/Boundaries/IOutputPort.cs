
using Microsoft.AspNetCore.Mvc;

namespace ServiceClock_BackEnd.Application.Boundaries;

public interface IOutputPort<T>
{
    IActionResult ViewModel { get; set; }

    void Standard(T output);
    void Error(string message);
    void NotFound(string message);
}

