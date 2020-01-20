using AutoMapper;
using ConferenceApp.Core.Models;
using ConferenceApp.Web.ViewModels;

namespace ConferenceApp.Web.Mapping
{
    public class AttachReportProfile : Profile
    {
        public AttachReportProfile() => CreateMap<ReportInnerModel, AttachViewModel>().ReverseMap();
    }
}