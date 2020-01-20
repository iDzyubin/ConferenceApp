using AutoMapper;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Web.Mapping
{
    public class UserShortInfoProfile : Profile
    {
        public UserShortInfoProfile()
        {
            CreateMap<User, UserShortInfoViewModel>()
                .ForMember( x => x.Id,
                    expression => expression.MapFrom( user => user.Id )
                )
                .ForMember( x => x.Email,
                    expression => expression.MapFrom( y => y.Email )
                )
                .ForMember( x => x.Name,
                    expression => expression.MapFrom( y => $"{y.LastName} {y.FirstName} {y.MiddleName}" )
                );
        }
    }
}