using FluentValidation;
using ServiceClock_BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Domain.Validations;
public class ClientValidator : AbstractValidator<Client>
{
    public ClientValidator()
    {
        RuleFor(c => c.Id)
           .NotEmpty().WithMessage("Id é obrigatório.");

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(100).WithMessage("Nome não pode ter mais de 100 caracteres.");

        RuleFor(c => c.PhoneNumber)
            .NotEmpty().WithMessage("Número de telefone é obrigatório.")
            .Matches(@"^[\d\s\+\-\(\)]+$") 
            .WithMessage("Número de telefone contém caracteres inválidos.");



        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail é inválido.");

        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(8).WithMessage("Senha deve ter no mínimo 8 caracteres.")
            .Matches(@"[A-Z]").WithMessage("Senha deve conter pelo menos uma letra maiúscula.")
            .Matches(@"[a-z]").WithMessage("Senha deve conter pelo menos uma letra minúscula.")
            .Matches(@"[0-9]").WithMessage("Senha deve conter pelo menos um número.")
            .Matches(@"[\W]").WithMessage("Senha deve conter pelo menos um caractere especial.")
            .When(c => c.Password.Length != 32 || !System.Text.RegularExpressions.Regex.IsMatch(c.Password, "^[a-f0-9]{32}$"))
            .WithMessage("A validação de senha é ignorada para senhas encriptadas.");

        RuleFor(c => c.Address)
            .NotEmpty().WithMessage("Endereço é obrigatório.")
            .MaximumLength(200).WithMessage("Endereço não pode ter mais de 200 caracteres.");

        RuleFor(c => c.City)
            .NotEmpty().WithMessage("Cidade é obrigatória.")
            .MaximumLength(50).WithMessage("Cidade não pode ter mais de 50 caracteres.");

        RuleFor(c => c.Country)
            .NotEmpty().WithMessage("País é obrigatório.")
            .MaximumLength(50).WithMessage("País não pode ter mais de 50 caracteres.");

        RuleFor(c => c.PostalCode)
            .NotEmpty().WithMessage("Código postal é obrigatório.");
    }
}
