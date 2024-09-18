
using ServiceClock_BackEnd.Domain.Enums;
using ServiceClock_BackEnd.Domain.Validations;

namespace ServiceClock_BackEnd.Domain.Models;

public class Message : Entity<Message, MessageValidator>
{
    public Message() 
        : base(new())
    {
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    public MessageType Type { get; set; }
    public Guid ClientId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid CreatedBy { get; set; }
    public string MessageContent { get; set; } = "";
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public bool Active { get; set; } = true;

    public Client? Client { get; set; }
    public Company? Company { get; set; }

}

