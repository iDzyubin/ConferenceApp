using System.Linq;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Extensions
{
    public static class ReportExtensions
    {
        public static ReportModel ConvertToReportModel( this Report report )
        {
            var model = new ReportModel
            {
                Title = report.Title,
                ReportId = report.Id,
                RequestId = report.RequestId,
                ReportStatus = report.Status,
                ReportType = report.ReportType,
                Collaborators = report.Collaboratorsreportidfkeys.ToList()
            };
            return model;
        }
        
        public static Report ConvertToReport( this ReportModel model )
        {
            var report = new Report
            {
                Id = model.ReportId,
                Status = model.ReportStatus,
                ReportType = model.ReportType,
                Title = model.Title,
                RequestId = model.RequestId,
                Collaboratorsreportidfkeys = model.Collaborators
            };
            return report;
        }
    }
}