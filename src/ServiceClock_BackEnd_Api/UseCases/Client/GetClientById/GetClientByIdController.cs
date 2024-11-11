using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;

namespace ServiceClock_BackEnd_Api.UseCases.Client.GetClientById;
[Route("api/[controller]")]
[ApiController]
public class GetClientByIdController : ControllerBase
{
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Client> repository;

    public GetClientByIdController(IRepository<ServiceClock_BackEnd.Domain.Models.Client> repository)
    {
        this.repository = repository;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public IActionResult Run([FromRoute] string id)
    {
        Guid.TryParse(id, out Guid clientId);
        if (clientId == Guid.Empty)
        {
            return new BadRequestObjectResult("Invalid Id");
        }
        var result = this.repository.Find(e => e.Id == clientId && e.Active == true)
        .Select(e => new
        {
            Id = e.Id, Name = e.Name, Address = e.Address, City = e.City, State = e.State,
            Country = e.Country, PostalCode = e.PostalCode, PhoneNumber = e.PhoneNumber, Email = e.Email, Image = e.ClientImage,
            BirthDate = e.BirthDate, CreatedAt = e.CreatedAt, CompanyId = e.CompanyId
        }).FirstOrDefault();

        if (result == null)
        {
            return new BadRequestObjectResult("Client not found");
        }

        return new OkObjectResult(new { Client = result});

    }
}
