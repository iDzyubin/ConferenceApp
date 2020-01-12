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
            RuleFor(x => x.Organization)
                .NotEmpty()
                .WithMessage("Organization title cannot be empty")
                .MaximumLength(150)
                .WithMessage("Organization title cannot be more than 150 characters");
            RuleFor(x => x.OrganisationAddress)
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
        }
    }
    
    public class UpdateUserValidator : AbstractValidator<UpdateUserViewModel>
    {
        public UpdateUserValidator()
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
            RuleFor(x => x.OrganisationAddress)
                .NotEmpty()
                .WithMessage("Organization address cannot be empty")
                .MaximumLength(500)
                .WithMessage("Address string cannot be more than 500 characters");
            RuleFor( x => x.City )
                .NotEmpty()
                .WithMessage( "City cannot be empty" )
                .MaximumLength( 150 )
                .WithMessage( "City cannot be more than 150 characters" );
        }
    }
}