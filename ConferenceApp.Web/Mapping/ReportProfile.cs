using AutoMapper;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Models;
using ConferenceApp.Web.ViewModels;

namespace ConferenceApp.Web.Mapping
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<ReportModel, ReportViewModel>().ReverseMap();

            CreateMap<Report, ReportViewModel>()
                .ForMember( x => x.Collaborators, expression 
                    => expression.MapFrom( y
                        => y.Collaboratorsreportidfkeys ) )
                .ForMember(x => x.ReportStatus, expression 
                    => expression.MapFrom( y 
                        => y.Status ) )
                .ForMember(x => x.ReportType, expression 
                    => expression.MapFrom( y 
                        => y.ReportType ) )
                .ReverseMap();

            CreateMap<Collaborator, UserShortInfoViewModel>()
                .ForMember( x => x.Id, expression 
                    => expression.MapFrom( y 
                        => y.UserId ) )
                .ForMember( x => x.Email, expression 
                    => expression.MapFrom( y 
                        => y.User.Email ) )
                .ForMember( x => x.Name, expression 
                    => expression.MapFrom( y
                        => $"{y.User.LastName} {y.User.FirstName} {y.User.MiddleName}" ) )
                .ReverseMap();
        }
    }
}