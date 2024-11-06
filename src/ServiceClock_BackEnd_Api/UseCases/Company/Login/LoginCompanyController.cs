using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.UseCases.Company.Login;

namespace ServiceClock_BackEnd_Api.UseCases.Company.Login;
[Route("api/[controller]")]
[ApiController]
public class LoginCompanyController : ControllerBase
{
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Company> repository;
    private readonly ITokenService tokenService;

    public LoginCompanyController
        (IRepository<ServiceClock_BackEnd.Domain.Models.Company> repository,
        ITokenService tokenService)
    {
        this.repository = repository;
        this.tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Run([FromBody] LoginCompanyRequest request)
    {
        if (request != null)
        {
            var company = this.repository.Find(e =>
            e.Email.Equals(request.Email)
            && e.Password.Equals(request.Password))
            .FirstOrDefault();

            if (company == null)
            {
                return new BadRequestObjectResult("Login Invalid");
            }
            var token = this.tokenService.Generate("Company", company.Id);
            return new OkObjectResult(new { UserId = company.Id, Token = token });
        }
        return new OkResult();

    }
}
