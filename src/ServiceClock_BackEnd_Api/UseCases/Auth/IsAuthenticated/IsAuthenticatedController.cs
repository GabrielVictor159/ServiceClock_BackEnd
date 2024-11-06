using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace ServiceClock_BackEnd_Api.UseCases.Auth.IsAuthenticated;
[Route("api/[controller]")]
[ApiController]
public class IsAuthenticatedController : ControllerBase
{
    [HttpGet]
    [HttpPost]
    public IActionResult Run()
    {
        return new OkObjectResult("User is authenticated successfully.");

    }

}
