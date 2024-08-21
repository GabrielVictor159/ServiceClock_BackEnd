
using FluentValidation;
using FluentValidation.Results;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceClock_BackEnd.Domain.Models;

public abstract class Entity<TModel, TValidator> : ICloneable
    where TValidator : AbstractValidator<TModel>
{
    [NotMapped]
    protected TValidator Validator { get; private set; }
    [NotMapped]
    public ValidationResult? ValidationResult { get; private set; }
    [NotMapped]
    public bool IsValid
    {
        get
        {
            ValidationResult = Validator.Validate((TModel)Clone());
            return ValidationResult.IsValid;
        }
        set { }
    }
    protected Entity(TValidator validator)
    {
        Validator = validator;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}

