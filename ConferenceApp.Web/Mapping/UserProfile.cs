using AutoMapper;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Web.ViewModels;

namespace ConferenceApp.Web.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile() => CreateMap<User, UserViewModel>().ReverseMap();
    }
}