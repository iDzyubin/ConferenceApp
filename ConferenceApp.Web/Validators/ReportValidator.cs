using ConferenceApp.Web.ViewModels;
using FluentValidation;

namespace ConferenceApp.Web.Validators
{
    public class ReportValidator : AbstractValidator<ReportViewModel>
    {
        public ReportValidator()
        {
            RuleFor( x => x.UserId )
                .NotEmpty()
                .WithMessage( "UserId cannot be empty" );

            RuleFor( x => x.Title )
                .NotEmpty()
                .WithMessage( "Title cannot be empty" )
                .MinimumLength( 5 )
                .WithMessage( "Title cannot be less than 5 characters" )
                .MaximumLength( 200 )
                .WithMessage( "Title cannot be more than 200 characters" );

            RuleFor( x => x.ReportType )
                .NotEmpty()
                .WithMessage( "Report type cannot be empty" );
            
            RuleFor( x => x.Collaborators )
                .NotEmpty()
                .WithMessage( "List of collaborators cannot be empty" );
        }
    }
}