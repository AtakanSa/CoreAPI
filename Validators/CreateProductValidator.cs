using CommonServiceLocator;
using FluentValidation;
using ProductCatalog.Contract.v1.Requests;
using ProductCatalog.Services;
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



            RuleFor(x => x.Code)
                 .NotEmpty().WithMessage("Code must not be empty !")
                 .Matches("^[a-zA-Z0-9]*$").WithMessage("Code can contains only alpha-numeric characters !");

            RuleFor(x => x.UpdatedAt)
                .NotEmpty().WithMessage("Update Time must not be empty !")
                .GreaterThanOrEqualTo(r => DateTime.Now).WithMessage("Date To must be after Date From");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name To must be after Date From");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .LessThan(1000)
                .NotEmpty()
                .WithMessage("Price must be correct format");



        }
    }
}
