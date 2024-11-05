using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Helpers.Hateoas;

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
    [Hateoas("Client", "search", "/GetClient/{id}", "GET")]
    public IActionResult Run([FromRoute] string id)
    {
        Guid.TryParse(id, out Guid companyId);
        if (companyId == Guid.Empty)
        {
            return new BadRequestObjectResult("Invalid Id");
        }
        var result = this.repository.Find(e => e.Id == companyId && e.Active == true)
        .Select(e => new
        {
            Id = e.Id, Password = e.Password, Name = e.Name, Address = e.Address, City = e.City, State = e.State,
            Country = e.Country, PostalCode = e.PostalCode, PhoneNumber = e.PhoneNumber, Email = e.Email, Image = e.ClientImage,
            BirthDate = e.BirthDate, CreatedAt = e.CreatedAt,
        }).FirstOrDefault();

        if (result == null)
        {
            return new BadRequestObjectResult("Client not found");
        }

        return new OkObjectResult(new { Client = result, _links = HateoasScheme.Instance.GetLinks("Client") });

    }
}
