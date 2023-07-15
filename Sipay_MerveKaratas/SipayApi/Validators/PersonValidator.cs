using FluentValidation;
using SipayApi.Models;

namespace SipayApi.Validators
{ 
    
    //fluent validationda validasyon yapmak için AbstractValidator sınıfından kalıtım alırız ve buraya tipi veiririz
    //böylece sadece o modele ait kuralları tutarak modüler bir yaklaşım gerçekleştiriş oluruz
    public class PersonValidator : AbstractValidator<Person>
    {// Person sınıfına ait fluent validation kurallarını validator sınıfına yazıyorum
        public PersonValidator()
        {
            RuleFor(a => a.Name)
           .NotEmpty().WithMessage("Staff person name is not empty.")
           .Length(5, 100).WithMessage("Staff person name must be between 5 and 100 characters.");

            RuleFor(a => a.Lastname)
                .NotEmpty().WithMessage("Staff person lastname is not empty.")
                .Length(5, 100).WithMessage("Staff person lastname must be between 5 and 100 characters.");

            //Regex
            RuleFor(a => a.Phone)
                .NotEmpty().WithMessage("Staff person phone number is not empty.")
                .Matches(@"^\d{10}$").WithMessage("Staff person phone number is not valid.");

            RuleFor(a => a.AccessLevel)
                .InclusiveBetween(1, 5).WithMessage("Staff person access level must be between 1 and 5.");

            RuleFor(a => a.Salary)
            .NotEmpty().WithMessage("Staff person salary is not empty.")
            .InclusiveBetween(5000, 50000).WithMessage("Staff person salary must be between 5000 and 50000.");

        }
    }
}
