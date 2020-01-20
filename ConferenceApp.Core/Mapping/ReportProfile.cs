using System;
using System.IO;
using System.Linq;
using AutoMapper;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Mapping
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<Report, ReportInnerModel>()
                .ForMember( x => x.FileName, expression
                    => expression.MapFrom( y
                        => Path.GetFileName( y.Path ) ) )
                .ForMember( x => x.Collaborators, expression
                    => expression.MapFrom( y
                        => y.Collaboratorsreportidfkeys.ToList() ) )
                .ReverseMap();

            CreateMap<Report, ReportOuterModel>()
                .ForMember( x => x.FileName, expression
                    => expression.MapFrom( y
                        => Path.GetFileName( y.Path ) ) )
                .ForMember( x => x.Collaborators, expression
                    => expression.MapFrom( y
                        => y.Collaboratorsreportidfkeys.ToList() ) )
                .ForMember(x => x.ReportStatus, expression 
                    => expression.MapFrom(y 
                        => y.Status))
                .ReverseMap();
            
            CreateMap<Collaborator, string>()
                .ConvertUsing( x
                    => ( String.IsNullOrEmpty( x.User.LastName ) ? String.Empty : $" {x.User.LastName}" ) +
                       ( String.IsNullOrEmpty( x.User.FirstName ) ? String.Empty : $" {x.User.FirstName}" ) +
                       ( String.IsNullOrEmpty( x.User.MiddleName ) ? String.Empty : $" {x.User.MiddleName}" )
                );
        }
    }
}