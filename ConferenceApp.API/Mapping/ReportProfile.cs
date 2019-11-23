using AutoMapper;
using ConferenceApp.API.ViewModels;
using ConferenceApp.Core.Extensions;
using ConferenceApp.Core.Models;

namespace ConferenceApp.API.Mapping
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<ReportViewModel, ReportModel>( )
                .ForMember( x => x.ReportId,
                    expression => expression.MapFrom( y => y.ReportId ) )
                .ForMember( x => x.RequestId,
                    expression => expression.MapFrom( y => y.RequestId ) )
                .ForMember( x => x.ReportStatus,
                    expression => expression.MapFrom( y => y.ReportStatus ) )
                .ForMember( x => x.ReportType,
                    expression => expression.MapFrom( y => y.ReportType ) )
                .ForMember( x => x.Title,
                    expression => expression.MapFrom( y => y.Title ) )
                .ForMember( x => x.File,
                    expression => expression.MapFrom( y => y.File.ConvertToFileStream() ) )
                .ForMember( x => x.Collaborators, 
                    expression => expression.MapFrom( y => y.Collaborators ) );
        }
    }
}