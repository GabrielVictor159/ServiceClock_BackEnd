
using ServiceClock_BackEnd.Application.Boundaries.Messages;

namespace ServiceClock_BackEnd.Api.UseCases.Messages.CreateMessage;

public class CreateMessageResponse : ResponseCore
{
    public CreateMessageResponse(CreateMessageBoundarie boundarie)
        : base("Message")
    {
        if(boundarie.Message!=null)
        {
            this.Id= boundarie.Message.Id;
        }
    }
    public Guid Id { get; set; }
}

