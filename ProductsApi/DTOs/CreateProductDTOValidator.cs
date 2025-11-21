using FluentValidation;

namespace ProductsApi.DTOs
{
    public class CreateProductDTOValidator : AbstractValidator<CreateProductDTO>
    {
        public CreateProductDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");
        }
    }
}
