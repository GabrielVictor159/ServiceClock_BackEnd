
using FluentValidation;
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Domain.Validations;

public class ServiceValidator : AbstractValidator<Service>
{
    public ServiceValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage("Id é obrigatório.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name é obrigatório.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description é obrigatório.");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address é obrigatório.");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City é obrigatório.");

        RuleFor(x => x.State)
            .NotEmpty()
            .WithMessage("State é obrigatório.");

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country é obrigatório.");

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage("PostalCode é obrigatório.");

        RuleFor(x => x.CreatedAt)
            .NotEmpty()
            .NotNull()
            .WithMessage("CreatedAt é obrigatório.");

        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .NotNull()
            .WithMessage("CompanyId é obrigatório.");

    }
}

