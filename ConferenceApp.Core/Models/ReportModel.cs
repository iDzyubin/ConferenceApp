using System;
using System.Collections.Generic;
using System.IO;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Models
{
    public class ReportModel
    {
        public Guid RequestId { get; set; }
        
        public Guid ReportId { get; set; }
        
        public string Title { get; set; }
        
        public ReportType ReportType { get; set; }
        
        public ReportStatus ReportStatus { get; set; }
        
        public FileStream File { get; set; }
        
        public string Collaborators { get; set; }
    }
}