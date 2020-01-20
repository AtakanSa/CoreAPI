using FluentValidation;
using ProductCatalog.Contract.v1.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidator()
        {

            var shopManagementService = ServiceLocator.Instance.GetService<IShopService>();

            RuleFor(x => x.Code)
                 .NotEmpty()
                 .Matches("^[a-zA-Z0-9]*$");
            RuleFor(x => x.UpdatedAt)
                .NotEmpty().WithMessage("*Required")
                .GreaterThanOrEqualTo(r => DateTime.Now)
                .WithMessage("Date To must be after Date From");
            RuleFor(x => x.Name)
                .NotEmpty();
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .LessThan(1000)
                .NotEmpty();
            RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Enter url.")
            .Length(4, 30).WithMessage("Length between 4 and 30 chars.")
            .Matches(@"[a-z\-\d]").WithMessage("Incorrect format.")
            .Must((model, url) => shopService.Available(url, model.ShopId)).WithMessage("Shop with this url already exists.");


        }
    }
}
