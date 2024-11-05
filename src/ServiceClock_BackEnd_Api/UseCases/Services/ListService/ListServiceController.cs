using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Helpers.Hateoas;
using ServiceClock_BackEnd.UseCases.Services.ListService;

namespace ServiceClock_BackEnd_Api.UseCases.Services.ListService;
[Route("api/[controller]")]
[ApiController]
public class ListServiceController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Client> clientRepository;
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Service> serviceRepository;

    public ListServiceController
        (IMapper mapper,
        IRepository<ServiceClock_BackEnd.Domain.Models.Client> clientRepository,
        IRepository<Service> serviceRepository)
    {
        this.mapper = mapper;
        this.clientRepository = clientRepository;
        this.serviceRepository = serviceRepository;
    }

    [HttpPost]
    [Hateoas("Service", "search", "/ListService", "POST", typeof(ListServiceRequest))]
    public async Task<IActionResult> Run(ListServiceRequest request)
    {
        var UserId = Guid.Parse(User.FindFirst("User_Id")!.Value);
        var UserType = User.FindFirst("User_Rule")!.Value;

        var companyId = Guid.Empty;
        if (UserType != "Company")
        {
            var client = this.clientRepository.Find(e => e.Id == UserId && e.Active == true).FirstOrDefault();
            if (client == null)
            {
                return new UnauthorizedResult();
            }
            companyId = client.CompanyId ?? Guid.Empty;

        }
        else
        {
            companyId = UserId;
        }
        if (request != null)
        {
            return new OkObjectResult(new
            {
                Services =
                this.serviceRepository.Find(e =>
                    (request.Id == Guid.Empty || e.Id == request.Id) &&
                    e.Name.ToLower().Contains(request.Name.ToLower()) &&
                    e.Description.ToLower().Contains(request.Description.ToLower()) &&
                    e.Address.ToLower().Contains(request.Address.ToLower()) &&
                    e.City.ToLower().Contains(request.City.ToLower()) &&
                    e.State.ToLower().Contains(request.State.ToLower()) &&
                    e.Country.ToLower().Contains(request.Country.ToLower()) &&
                    e.PostalCode.ToLower().Contains(request.PostalCode.ToLower()) &&
                    e.CompanyId == companyId
                , request.IndexPage, ((int)request.PageSize))
                .Select(e => new
                {
                    Id = e.Id, Name = e.Name, Description = e.Description, Address = e.Address,
                    City = e.City, State = e.State, Country = e.Country, PostalCode = e.PostalCode,
                })
            , _links = HateoasScheme.Instance.GetLinks("Service")
            });
        }
        return new OkResult();
    }
}
