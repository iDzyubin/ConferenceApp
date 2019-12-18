using AutoMapper;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Mapping
{
    public class ReportProfile : Profile
    {
        public ReportProfile() => CreateMap<Report, ReportModel>().ReverseMap();
    }
}