using FluentValidation;
using MediatorSample.WebApi.Application.Features.ProductFeatures.Commands;

namespace MediatorSample.WebApi.Validators
{
    public class CreateProductCommndValidator: AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommndValidator()
        {
            RuleFor(c => c.Barcode).NotEmpty().WithMessage("O campo Barcode deve ser preenchido!");
            RuleFor(c => c.Name).NotEmpty().WithMessage("O campo Nome deve ser preenchido!");
        }

    }
}
