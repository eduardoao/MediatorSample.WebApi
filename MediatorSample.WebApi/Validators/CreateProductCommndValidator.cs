using FluentValidation;
using MediatorSample.WebApi.Application.Features.ProductFeatures.Commands;

namespace MediatorSample.WebApi.Validators
{
    public class CreateProductCommndValidator: AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommndValidator()
        {
            RuleFor(c => c.Barcode).NotEmpty();
            RuleFor(c => c.Name).NotEmpty();
        }

    }
}
