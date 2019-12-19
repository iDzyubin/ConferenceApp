using System;
using System.Collections.Generic;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Web.ViewModels
{
    public class AttachViewModel
    {
        public Guid UserId { get; set; }
        
        public string Title { get; set; }
        
        public ReportType ReportType { get; set; }
        
        public ReportStatus ReportStatus { get; set; }
        
        public List<string> Collaborators { get; set; }
    }
}