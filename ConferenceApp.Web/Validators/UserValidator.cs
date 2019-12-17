using ConferenceApp.Web.ViewModels;
using FluentValidation;

namespace ConferenceApp.Web.Validators
{
    public class UserValidator : AbstractValidator<UserViewModel>
    {
        public UserValidator()
        {
            RuleFor( x => x.FirstName )
                .NotEmpty()
                .WithMessage("First name cannot be empty");
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name cannot be empty");
            RuleFor(x => x.Degree)
                .NotEmpty()
                .WithMessage("Degree cannot be empty");
            RuleFor(x => x.Organization)
                .NotEmpty()
                .WithMessage("Organization title cannot be empty")
                .MaximumLength(150)
                .WithMessage("Organization title cannot be more than 150 characters");
            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address cannot be empty")
                .MaximumLength(500)
                .WithMessage("Address string cannot be more than 500 characters");
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email cannot be empty")
                .EmailAddress()
                .WithMessage("This is not email address");
        }
    }
}