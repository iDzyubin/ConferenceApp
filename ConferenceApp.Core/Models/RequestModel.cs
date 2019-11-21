using System.Collections.Generic;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Models
{
    public class RequestModel
    {
        public User User { get; set; }
        
        public List<ReportModel> Reports { get; set; }
    }
}