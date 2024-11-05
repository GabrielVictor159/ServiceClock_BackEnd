
using ServiceClock_BackEnd.Application.Boundaries.Messages;

namespace ServiceClock_BackEnd.UseCases.Messages.CreateMessage;

public class CreateMessageResponse
{
    public CreateMessageResponse(CreateMessageBoundarie boundarie)
    {
        if(boundarie.Message!=null)
        {
            this.Id= boundarie.Message.Id;
        }
    }
    public Guid Id { get; set; }
}

