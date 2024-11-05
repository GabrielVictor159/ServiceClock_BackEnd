using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceClock_BackEnd.Application.Interfaces.Repositories;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Helpers.Hateoas;
using ServiceClock_BackEnd.UseCases.Messages.DeleteMessage;

namespace ServiceClock_BackEnd_Api.UseCases.Messages.DeleteMessage;
[Route("api/[controller]")]
[ApiController]
public class DeleteMessageController : ControllerBase
{
    private readonly IRepository<ServiceClock_BackEnd.Domain.Models.Message> repository;
    private readonly IBlobService blobService;

    public DeleteMessageController(IRepository<Message> repository, IBlobService blobService)
    {
        this.repository = repository;
        this.blobService = blobService;
    }

    [HttpPost]
    [Hateoas("Message", "delete", "/DeleteMessage", "POST", typeof(DeleteMessageRequest))]
    public IActionResult Run(DeleteMessageRequest request)
    {
        var UserId = Guid.Parse(User.FindFirst("User_Id")!.Value);
        var message = this.repository.Find(e => e.Id == request.MessageId && e.Active == true).FirstOrDefault();
        if (message == null)
        {
            return new BadRequestObjectResult("Message not found");
        }
        if (message.CreatedBy != UserId)
        {
            return new ForbidResult("Você não tem permissão para excluir essa mensagem");
        }
        if (message.Type != MessageType.Txt)
        {
            blobService.MoveBlobToPrivateContainer(message.MessageContent);
        }
        message.Active = false;
        this.repository.Update(message);

        return new OkResult();
    }
}
