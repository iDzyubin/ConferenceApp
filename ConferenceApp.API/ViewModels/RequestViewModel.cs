using System.Collections.Generic;
using ConferenceApp.API.Models;
using User = ConferenceApp.Core.DataModels.User;

namespace ConferenceApp.API.ViewModels
{
    public class RequestViewModel
    {
        public User User { get; set; }
        
        public List<ReportModel> Reports { get; set; }
    }
}