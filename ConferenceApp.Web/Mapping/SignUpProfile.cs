using AutoMapper;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Web.ViewModels;

namespace ConferenceApp.Web.Mapping
{
    public class SignUpProfile : Profile
    {
        public SignUpProfile() => CreateMap<User, SignUpViewModel>().ReverseMap();
    }
}