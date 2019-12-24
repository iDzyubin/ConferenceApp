using ConferenceApp.Web.ViewModels;
using FluentValidation;

namespace ConferenceApp.Web.Validators
{
    public class SignUpValidator : AbstractValidator<SignUpViewModel>
    {
        public SignUpValidator()
        {
            RuleFor( x => x.FirstName )
                .NotEmpty()
                .WithMessage("First name cannot be empty");
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name cannot be empty");
            RuleFor(x => x.Organization)
                .NotEmpty()
                .WithMessage("Organization title cannot be empty")
                .MaximumLength(150)
                .WithMessage("Organization title cannot be more than 150 characters");
            RuleFor(x => x.OrganizationAddress)
                .NotEmpty()
                .WithMessage("Organization address cannot be empty")
                .MaximumLength(500)
                .WithMessage("Address string cannot be more than 500 characters");
            RuleFor( x => x.City )
                .NotEmpty()
                .WithMessage( "City cannot be empty" )
                .MaximumLength( 150 )
                .WithMessage( "City cannot be more than 150 characters" );
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email cannot be empty")
                .EmailAddress()
                .WithMessage("This is not email address");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password cannot be empty")
                .MinimumLength(4)
                .WithMessage("Password cannot be less 4 characters");
        }
    }
}