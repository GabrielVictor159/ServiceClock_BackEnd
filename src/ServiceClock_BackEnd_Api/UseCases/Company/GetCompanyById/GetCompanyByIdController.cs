using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;

namespace ServiceClock_BackEnd_Api.UseCases.Company.GetCompanyById;
[Route("api/[controller]")]
[ApiController]
public class GetCompanyByIdController : ControllerBase
{
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Company> repository;

    public GetCompanyByIdController(IRepository<ServiceClock_BackEnd.Domain.Models.Company> repository)
    {
        this.repository = repository;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public IActionResult Run([FromRoute] string id)
    {
        Guid.TryParse(id, out Guid companyId);
        if (companyId == Guid.Empty)
        {
            return new BadRequestObjectResult("Invalid Id");
        }
        var result = this.repository.Find(e => e.Id == companyId)
        .Select(e => new
        {
            Id = e.Id, Name = e.Name, RegistrationNumber = e.RegistrationNumber, Address = e.Address, City = e.City, State = e.State,
            Country = e.Country, PostalCode = e.PostalCode, PhoneNumber = e.PhoneNumber, Email = e.Email, Image = e.CompanyImage
        }).FirstOrDefault();

        if (result == null)
        {
            return new BadRequestObjectResult("Company not found");
        }

        return new OkObjectResult(new { Company = result});

    }
}
