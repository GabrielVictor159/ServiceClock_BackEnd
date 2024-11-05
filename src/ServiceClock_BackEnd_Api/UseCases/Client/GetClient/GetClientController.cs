using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.UseCases.Client.GetClient;

namespace ServiceClock_BackEnd_Api.UseCases.Client.GetClient;
[Route("api/[controller]")]
[ApiController]
public class GetClientController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Client> repository;

    public GetClientController
        (IMapper mapper,
        IRepository<ServiceClock_BackEnd.Domain.Models.Client> repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    [HttpPost]
    public IActionResult Run([FromBody] GetClientRequest request)
    {
        var UserId = Guid.Parse(User.FindFirst("User_Id")!.Value);
        var UserType = User.FindFirst("User_Rule")!.Value;
        if (UserType == "Client")
        {
            return new ForbidResult("Você não tem permissão para listar os clientes");
        }

        if (request != null)
        {
            var result = this.repository.Find(e =>
                    e.Active == true &&
                    (request.Id == Guid.Empty || e.Id == request.Id) &&
                    e.CompanyId == UserId &&
                    e.Name.ToLower().Contains(request.Name.ToLower()) &&
                    e.PhoneNumber.ToLower().Contains(request.PhoneNumber.ToLower()) &&
                    e.Address.ToLower().Contains(request.Address.ToLower()) &&
                    e.City.ToLower().Contains(request.City.ToLower()) &&
                    e.State.ToLower().Contains(request.State.ToLower()) &&
                    e.Country.ToLower().Contains(request.Country.ToLower()) &&
                    e.PostalCode.ToLower().Contains(request.PostalCode.ToLower()) &&
                    e.Email.ToLower().Contains(request.Email.ToLower()) &&
                    e.Active == true
                , request.IndexPage, ((int)request.PageSize))
                .Select(e => new
                {
                    Id = e.Id, Name = e.Name, Address = e.Address, City = e.City, State = e.State,
                    Country = e.Country, PostalCode = e.PostalCode, PhoneNumber = e.PhoneNumber,
                    Email = e.Email, BirthDate = e.BirthDate, CreatedAt = e.CreatedAt,
                    Image = e.ClientImage
                });
            return new OkObjectResult(
                new { Clients = result }
            );
        }
        return new OkResult();
    }
}
