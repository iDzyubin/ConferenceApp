using ConferenceApp.Web.ViewModels;
using FluentValidation;

namespace ConferenceApp.Web.Validators
{
    public class ReportValidator : AbstractValidator<ReportViewModel>
    {
        public ReportValidator()
        {
            RuleFor( x => x.File )
                .NotEmpty()
                .WithMessage( "File does not attached" );
            RuleFor( x => x.Collaborators )
                .NotEmpty()
                .WithMessage( "Count of report's collaborators is 0" );
        }
    }
}