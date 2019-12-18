using ConferenceApp.Web.ViewModels;
using FluentValidation;

namespace ConferenceApp.Web.Validators
{
    public class SignInValidator : AbstractValidator<SignInViewModel>
    {
        public SignInValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            
            RuleFor( x => x.Email )
                .NotEmpty()
                .WithMessage( "Email cannot be empty" )
                .EmailAddress()
                .WithMessage( "This is not email address" );

            RuleFor( x => x.Password )
                .NotEmpty()
                .WithMessage("Password cannot be empty");
        }        
    }
}