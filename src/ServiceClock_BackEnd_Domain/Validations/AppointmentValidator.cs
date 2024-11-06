
using FluentValidation;
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Domain.Validations;

public class AppointmentValidator : AbstractValidator<Appointment>
{
    public AppointmentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id é obrigatório.");

        RuleFor(x => x.ServiceId)
            .NotEmpty()
            .WithMessage("ServiceId é obrigatório.");

        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithMessage("ClientId é obrigatório.");

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date é obrigatório.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description é obrigatório.");

        RuleFor(x => x.Status)
            .NotNull()
            .WithMessage("Status é obrigatório.");

        RuleFor(x => x.CreatedAt)
            .NotEmpty()
            .NotNull()
            .WithMessage("CreatedAt é obrigatório.");
    }
}

