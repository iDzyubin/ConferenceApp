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
                Collaborators = report.Collaborators
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
                Collaborators = model.Collaborators
            };
            return report;
        }
    }
}