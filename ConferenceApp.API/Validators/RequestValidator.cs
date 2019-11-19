using ConferenceApp.API.ViewModels;
using FluentValidation;

namespace ConferenceApp.API.Validators
{
    public class RequestValidator : AbstractValidator<RequestViewModel>
    {
        public RequestValidator()
        {
            RuleFor( x => x.User )
                .NotEmpty()
                .WithMessage( "File does not attached" );
            RuleFor( x => x.Reports )
                .NotEmpty()
                .WithMessage( "Count of report's collaborators is 0" );
        }
    }
}