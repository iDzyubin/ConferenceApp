using System;
using System.Collections.Generic;
using ConferenceApp.Core.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceApp.Web.ViewModels
{
    public class ReportViewModel
    {
        public Guid UserId { get; set; }
        
        public string Title { get; set; }
        
        public ReportType ReportType { get; set; }
        
        public ReportStatus ReportStatus { get; set; }
        
        public List<UserShortInfoViewModel> Collaborators { get; set; }
    }
}