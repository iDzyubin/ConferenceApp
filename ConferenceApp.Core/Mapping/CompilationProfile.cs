using System.IO;
using AutoMapper;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Mapping
{
    public class CompilationProfile : Profile
    {
        public CompilationProfile()
        {
            CreateMap<Compilation, CompilationModel>()
                .ForMember( x => x.Title, expression 
                    => expression.MapFrom( y 
                        => Path.GetFileName( y.Path ) ) )
                .ReverseMap();
        }
    }
}