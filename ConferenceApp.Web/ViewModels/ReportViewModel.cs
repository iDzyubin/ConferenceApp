using System;
using System.Collections.Generic;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Web.ViewModels
{
    public class ReportViewModel
    {
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }
        
        public string Title { get; set; }
        
        public ReportType ReportType { get; set; }
        
        public ReportStatus ReportStatus { get; set; }
        
        public List<UserShortInfoViewModel> Collaborators { get; set; }
        
        public Guid SectionId { get; set; }
        
        public string FileName { get; set; }
    }
}