using FluentValidation;
using ServiceClock_BackEnd.Domain.Models;
namespace ServiceClock_BackEnd.Domain.Validations;

public class CompanyValidator : AbstractValidator<Company>
{
    public CompanyValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id é obrigatório.");

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(100).WithMessage("Nome não pode ter mais de 100 caracteres.");

        RuleFor(c => c.RegistrationNumber)
            .NotEmpty().WithMessage("Número de registro é obrigatório.")
            .Matches(@"^[A-Za-z0-9\-]+$").WithMessage("Número de registro é inválido.");

        RuleFor(c => c.Address)
            .NotEmpty().WithMessage("Endereço é obrigatório.")
            .MaximumLength(200).WithMessage("Endereço não pode ter mais de 200 caracteres.");

        RuleFor(c => c.City)
            .NotEmpty().WithMessage("Cidade é obrigatória.")
            .MaximumLength(50).WithMessage("Cidade não pode ter mais de 50 caracteres.");

        RuleFor(c => c.State)
            .NotEmpty().WithMessage("Estado é obrigatório.")
            .MaximumLength(50).WithMessage("Estado não pode ter mais de 50 caracteres.");

        RuleFor(c => c.Country)
            .NotEmpty().WithMessage("País é obrigatório.")
            .MaximumLength(50).WithMessage("País não pode ter mais de 50 caracteres.");

        RuleFor(c => c.PostalCode)
            .NotEmpty().WithMessage("Código postal é obrigatório.")
            .Matches(@"^\d{5}-\d{4}$").WithMessage("Código postal deve estar no formato XXXXX-XXXX.");

        RuleFor(c => c.PhoneNumber)
            .NotEmpty().WithMessage("Número de telefone é obrigatório.")
            .Matches(@"^\+?\d{10,15}$").WithMessage("Número de telefone é inválido.");

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail é inválido.");

        RuleFor(c => c.EstablishedDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Data de fundação não pode ser no futuro.");
    }
}


