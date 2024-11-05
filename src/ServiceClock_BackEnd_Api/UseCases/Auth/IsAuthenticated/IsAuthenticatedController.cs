using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Helpers.Hateoas;

namespace ServiceClock_BackEnd_Api.UseCases.Auth.IsAuthenticated;
[Route("api/[controller]")]
[ApiController]
public class IsAuthenticatedController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    [HttpPost]
    [Hateoas("Auth", "auth", "/IsAuthenticated", "GET", typeof(object))]
    public IActionResult Run()
    {
        return new OkObjectResult("User is authenticated successfully.");

    }

}
