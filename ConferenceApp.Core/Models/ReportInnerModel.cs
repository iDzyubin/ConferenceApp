using System;
using System.Collections.Generic;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Models
{
    public class ReportInnerModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        
        public string Title { get; set; }
        
        public ReportType ReportType { get; set; }
        
        public ReportStatus ReportStatus { get; set; }
        
        public List<string> Collaborators { get; set; }
        
        public Guid SectionId { get; set; }
        
        public string FileName { get; set; }
    }
}