using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Helpers.Hateoas;
using ServiceClock_BackEnd.UseCases.Messages.ListMessage;

namespace ServiceClock_BackEnd_Api.UseCases.Messages.ListMessage;
[Route("api/[controller]")]
[ApiController]
public class ListMessageController : ControllerBase
{
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Message> repository;
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Client> repositoryClient;

    public ListMessageController
        (IRepository<Message> repository,
        IRepository<ServiceClock_BackEnd.Domain.Models.Client> repositoryClient)
    {
        this.repository = repository;
        this.repositoryClient = repositoryClient;
    }

    [HttpPost]
    [Hateoas("Message", "search", "/ListMessage", "Post", typeof(ListMessageRequest))]
    public IActionResult Run(ListMessageRequest request)
    {
        var UserId = Guid.Parse(User.FindFirst("User_Id")!.Value);
        var UserType = User.FindFirst("User_Rule")!.Value;

        if (UserType == "Client")
        {
            request.ClientId = UserId;
        }
        if (UserType == "Company")
        {
            request.CompanyId = UserId;
        }

        var client = repositoryClient.Find(e => e.Id == request.ClientId && e.Active == true).FirstOrDefault();
        if (client == null)
        {
            return new BadRequestObjectResult("Cliente não encontrado");
        }
        return new OkObjectResult(new
        {
            Messages =
            this.repository
            .Find(e => e.ClientId == request.ClientId && e.CompanyId == request.CompanyId && (request.MinDate == null ? true : e.CreateAt > request.MinDate) && e.Active == true)
            .OrderByDescending(e => e.CreateAt)
            .Select(e => new
            {
                Id = e.Id,
                Type = e.Type,
                ClientId = e.ClientId,
                CompanyId = e.CompanyId,
                CreatedBy = e.CreatedBy,
                MessageContent = e.MessageContent,
                CreateAt = e.CreateAt,
            }),

            _links = HateoasScheme.Instance.GetLinks("Message")

        });
    }
}
