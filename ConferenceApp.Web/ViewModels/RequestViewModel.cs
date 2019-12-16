using System.Collections.Generic;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Web.ViewModels
{
    public class RequestViewModel
    {
        public UserModel User { get; set; }
        
        public List<ReportModel> Reports { get; set; }
    }
}