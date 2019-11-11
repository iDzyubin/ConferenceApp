using AutoMapper;
using ConferenceApp.API.Extensions;
using ConferenceApp.API.Models;
using dto = ConferenceApp.Core.DataModels;

namespace ConferenceApp.API.Mapping
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<Report, dto.Report>()
                .ForMember( x => x.Title, 
                    expression => expression.MapFrom( y => y.Title ) 
                )
                .ForMember( x => x.Collaboratorsreportidfkeys,
                    expression => expression.MapFrom( y => y.Collaborators.ConvertToCollaborators() )
                );
        }
    }
}