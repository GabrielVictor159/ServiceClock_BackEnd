using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Boundaries.Messages;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.UseCases.Messages.CreateMessage;
using ServiceClock_BackEnd.Helpers.Hateoas;
using ServiceClock_BackEnd.UseCases.Messages.CreateMessage;
using ServiceClock_BackEnd_Application.Interfaces;

namespace ServiceClock_BackEnd_Api.UseCases.Messages.CreateMessage;
[Route("api/[controller]")]
[ApiController]
public class CreateMessageController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IOutputPort<CreateMessageBoundarie> presenter;
    private readonly ICreateMessageUseCase useCase;
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Client> clientRepository;

    public CreateMessageController
        (IMapper mapper, 
        IOutputPort<CreateMessageBoundarie> presenter, 
        ICreateMessageUseCase useCase, 
        IRepository<ServiceClock_BackEnd.Domain.Models.Client> clientRepository)
    {
        this.mapper = mapper;
        this.presenter = presenter;
        this.useCase = useCase;
        this.clientRepository = clientRepository;
    }

    [HttpPost]
    [Hateoas("Message", "create", "/CreateMessage", "POST", typeof(CreateMessageRequest))]
    public IActionResult Run(CreateMessageRequest request)
    {
            if (request != null)
            {
                var UserId = Guid.Parse(User.FindFirst("User_Id")!.Value);
                var UserType = User.FindFirst("User_Rule")!.Value;

                request.CreatedBy = UserId;

                if (UserType == "Client")
                {
                    var client = this.clientRepository.Find(e => e.Id == UserId && e.Active == true).FirstOrDefault();
                    if (client == null)
                    {
                        return new BadRequestObjectResult("Client not found");
                    }
                    request.ClientId = client.Id;
                    request.CompanyId = client.CompanyId ?? request.CompanyId;
                }

                if (UserType == "Company")
                {
                    request.CompanyId = UserId;
                    var client = this.clientRepository.Find(e => e.Id == request.ClientId && e.CompanyId == request.CompanyId && e.Active == true).FirstOrDefault();
                    if (client == null)
                    {
                        return new BadRequestObjectResult("Client not found");
                    }
                }

                var requestUseCase = this.mapper.Map<CreateMessageUseCaseRequest>(request);
                this.useCase.Execute(requestUseCase);
            }
            return this.presenter.ViewModel;
    }
}
