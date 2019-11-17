using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using dto = ConferenceApp.Core.DataModels;

namespace ConferenceApp.API.Models
{
    public class Report
    {
        public string         Title         { get; set; }
        public IFormFile      File          { get; set; }
        public List<dto.Collaborator> Collaborators { get; set; }
    }
}