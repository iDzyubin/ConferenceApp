//using AutoMapper;
//using ConferenceApp.API.Extensions;
//using ConferenceApp.API.Models;
//using ConferenceApp.Core.Models;
//using dto = ConferenceApp.Core.DataModels;
//
//namespace ConferenceApp.API.Mapping
//{
//    public class ReportProfile : Profile
//    {
//        public ReportProfile()
//        {
//            CreateMap<ReportModel, dto.Report>()
//                .ForMember( x => x.Title,
//                    expression => expression.MapFrom( y => y.Title )
//                )
//                .ForMember( x => x.ReportType,
//                    expression => expression.MapFrom( y => y.ReportType )
//                )
//                .ForMember( x => x.Collaboratorsreportidfkeys,
//                    expression => expression.MapFrom( y => y.Collaborators )
//                );
//        }
//    }
//}