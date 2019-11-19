using System.Collections.Generic;

namespace ConferenceApp.API.Models
{
    public class RequestModel
    {
        public Core.DataModels.User User { get; set; }
        
        public List<ReportModel> Reports { get; set; }
    }
}