using System;
using System.Collections.Generic;
using ConferenceApp.Core.DataModels;
using Microsoft.AspNetCore.Http;

namespace ConferenceApp.API.ViewModels
{
    public class ReportViewModel
    {
        public Guid RequestId { get; set; }
        
        public Guid ReportId { get; set; }
        
        public string Title { get; set; }
        
        public ReportType ReportType { get; set; }
        
        public ReportStatus ReportStatus { get; set; }
        
        public IFormFile File { get; set; }
        
        public List<Collaborator> Collaborators { get; set; }
    }
}