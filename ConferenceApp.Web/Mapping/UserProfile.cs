using AutoMapper;
using ConferenceApp.Web.Models;
using dto = ConferenceApp.Core.DataModels;

namespace ConferenceApp.Web.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, dto.Admin>()
                .ForMember( x => x.Login, 
                    expression => expression.MapFrom( y => y.Username ) )
                .ForMember( x => x.Password, 
                    expression => expression.MapFrom( y => y.Password ) );
        }
    }
}