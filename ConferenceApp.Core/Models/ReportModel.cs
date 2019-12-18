using System;
using System.Collections.Generic;
using System.IO;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Models
{
    public class ReportModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        
        public string Title { get; set; }
        
        public ReportType ReportType { get; set; }
        
        public ReportStatus ReportStatus { get; set; }
        
        public List<string> Collaborators { get; set; }
    }
}