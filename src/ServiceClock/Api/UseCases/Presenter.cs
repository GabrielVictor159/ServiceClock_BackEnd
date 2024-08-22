
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Boundaries;

namespace ServiceClock_BackEnd.Api.UseCases;

public abstract class Presenter<Request, Response> : IOutputPort<Request>
{
    public IActionResult ViewModel { get; set; } = new ObjectResult(new { StatusCode = 500 });

    public void Error(string message)
    {
        var problemdetails = new ProblemDetails()
        {
            Status = 500,
            Detail = message
        };
        ViewModel = new BadRequestObjectResult(problemdetails);
    }

    public void NotFound(string message)
    {
        ViewModel = new NotFoundObjectResult(message);
    }

    public void Standard(Request request)
    {
        var response = Activator.CreateInstance(typeof(Response), request);
        this.ViewModel = new OkObjectResult(response);
    }
}

