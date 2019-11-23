using AutoMapper;
using ConferenceApp.API.ViewModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.API.Mapping
{
    public class RequestProfile : Profile
    {
        public RequestProfile()
        {
            CreateMap<RequestViewModel, RequestModel>( )
                .ForMember( x => x.User,
                    expression => expression.MapFrom( y => y.User ) )
                .ForMember( x => x.Reports, 
                    expression => expression.MapFrom( y => y.Reports ) );
        }
    }
}