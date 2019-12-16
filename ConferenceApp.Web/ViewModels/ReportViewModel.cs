using System;
using ConferenceApp.Core.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceApp.Web.ViewModels
{
    public class ReportViewModel
    {
        public Guid RequestId { get; set; }
        
        public Guid ReportId { get; set; }
        
        public string Title { get; set; }
        
        public ReportType ReportType { get; set; }
        
        public ReportStatus ReportStatus { get; set; }
        
        public IFormFile File { get; set; }
        
        public string Collaborators { get; set; }
        
        public DateTime? StartResidenceDate { get; set; }
        
        public DateTime? EndResidenceDate   { get; set; }
    }
}