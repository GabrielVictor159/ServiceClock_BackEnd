
using Newtonsoft.Json;
using ServiceClock_BackEnd.Domain.Enums;

namespace ServiceClock_BackEnd.Application.UseCases.Messages.CreateMessage;

public class CreateMessageUseCaseRequest
{
    public CreateMessageUseCaseRequest(MessageType type, Guid clientId, Guid companyId, Guid createdBy, string content, string fileName)
    {
        this.Message = new Domain.Models.Message()
        {
            Id = Guid.NewGuid(),
            Type = type,
            ClientId = clientId,
            CompanyId = companyId,
            CreatedBy = createdBy,
            MessageContent = (int)type>1? "": content,
        };

        Content = content;
        FileName = fileName;
    }

    public string Content { get; set; } = "";
    public string FileName { get; set; } = "";

    public Domain.Models.Message Message { get; set; }
}

