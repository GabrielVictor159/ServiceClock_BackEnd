
using FluentValidation;
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Domain.Validations;

public class MessageValidator : AbstractValidator<Message>
{
    public MessageValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id é obrigatório.");

        RuleFor(x => x.Type)
            .NotNull()
            .WithMessage("Type é obrigatório.");

        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithMessage("ClientId é obrigatório.");

        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage("CompanyId é obrigatório.");

        RuleFor(x => x.MessageContent)
            .NotEmpty()
            .WithMessage("Message Content é obrigatório.")
            .MaximumLength(200)
            .WithMessage("O tamanho maximo da mensagem é 200 Carácteres");

        RuleFor(x => x.LogDate)
            .NotEmpty()
            .WithMessage("LogDate é obrigatório.");
    }
}

