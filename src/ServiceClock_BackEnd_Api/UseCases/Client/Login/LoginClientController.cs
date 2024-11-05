using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Helpers.Hateoas;
using ServiceClock_BackEnd.UseCases.Client.Login;

namespace ServiceClock_BackEnd_Api.UseCases.Client.Login;
[Route("api/[controller]")]
[ApiController]
public class LoginClientController : ControllerBase
{
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Client> repository;
    private readonly ITokenService tokenService;

    public LoginClientController
        (IRepository<ServiceClock_BackEnd.Domain.Models.Client> repository,
        ITokenService tokenService)
    {
        this.repository = repository;
        this.tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost]
    [Hateoas("Client", "related", "/LoginClient", "POST", typeof(LoginClientRequest))]
    public IActionResult Run(LoginClientRequest request)
    {
        if (request != null)
        {
            var client = this.repository.Find(e =>
            e.Email.Equals(request.Email)
            && e.Password.Equals(request.Password)
            && e.Active == true)
            .FirstOrDefault();

            if (client == null)
            {
                return new BadRequestObjectResult("Login Invalid");
            }
            var token = this.tokenService.Generate("Client", client.Id);
            return new OkObjectResult(new { UserId = client.Id, Token = token, _links = HateoasScheme.Instance.GetLinks("Client") });
        }
        return new OkResult();
    }
}
