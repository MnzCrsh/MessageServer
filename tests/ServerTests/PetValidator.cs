using System.Data;
using FluentValidation;
using MessageServer.Domain;

namespace ServerTests;

public class PetValidator : AbstractValidator<Pet>
{
    public PetValidator()
    {
        RuleFor(pet => pet.Name)
            .NotEmpty()
            .WithMessage("Name is required");
        RuleFor(pet => pet.PetAge)
            .InclusiveBetween(1, 100)
            .WithMessage("Pet age must be between 1 and 100.");
    }
}