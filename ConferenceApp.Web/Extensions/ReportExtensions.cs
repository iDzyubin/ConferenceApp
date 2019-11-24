using ConferenceApp.Core.Extensions;
using ConferenceApp.Core.Models;
using ConferenceApp.Web.ViewModels;

namespace ConferenceApp.Web.Extensions
{
    public static class ReportExtensions
    {
        public static ReportModel ConvertToReportModel( this ReportViewModel model )
            => new ReportModel
            {
                RequestId = model.RequestId,
                Title = model.Title,
                ReportType = model.ReportType,
                File = model.File.ConvertToFileStream(),
                Collaborators = model.Collaborators
            };

        public static ReportViewModel ConvertToReportViewModel( this ReportModel report )
            => new ReportViewModel
            {
                Title = report.Title,
                ReportId = report.ReportId,
                RequestId = report.RequestId,
                ReportStatus = report.ReportStatus,
                ReportType = report.ReportType,
                Collaborators = report.Collaborators
            };
    }
}