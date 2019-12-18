using AutoMapper;
using ConferenceApp.Core.Models;
using ConferenceApp.Web.ViewModels;

namespace ConferenceApp.Web.Mapping
{
    public class ReportProfile : Profile
    {
        public ReportProfile() => CreateMap<ReportModel, ReportViewModel>().ReverseMap();
    }
}