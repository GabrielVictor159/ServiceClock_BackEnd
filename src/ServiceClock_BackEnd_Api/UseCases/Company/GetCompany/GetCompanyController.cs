using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.UseCases.Company.GetCompany;

namespace ServiceClock_BackEnd_Api.UseCases.Company.GetCompany;
[Route("api/[controller]")]
[ApiController]
public class GetCompanyController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Company> repository;

    public GetCompanyController
        (IMapper mapper,
        IRepository<ServiceClock_BackEnd.Domain.Models.Company>
        repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    [HttpPost]
    public IActionResult Run([FromBody] GetCompanyRequest request)
    {
        if (request != null)
        {

            return new OkObjectResult(new
            {
                Companies =
                this.repository.Find(e =>
                    (request.Id == Guid.Empty || e.Id == request.Id) &&
                    e.Name.ToLower().Contains(request.Name.ToLower()) &&
                    e.RegistrationNumber.ToLower().Contains(request.RegistrationNumber.ToLower()) &&
                    e.Address.ToLower().Contains(request.Address.ToLower()) &&
                    e.City.ToLower().Contains(request.City.ToLower()) &&
                    e.State.ToLower().Contains(request.State.ToLower()) &&
                    e.Country.ToLower().Contains(request.Country.ToLower()) &&
                    e.PostalCode.ToLower().Contains(request.PostalCode.ToLower()) &&
                    e.PhoneNumber.ToLower().Contains(request.PhoneNumber.ToLower()) &&
                    e.Email.ToLower().Contains(request.Email.ToLower())
                , request.IndexPage, ((int)request.PageSize))
                .Select(e => new
                {
                    Name = e.Name, RegistrationNumber = e.RegistrationNumber, Address = e.Address, City = e.City, State = e.State,
                    Country = e.Country, PostalCode = e.PostalCode, PhoneNumber = e.PhoneNumber, Email = e.Email, Image = e.CompanyImage
                }), 
            }
            );
        }
        return new OkResult();
    }
}
