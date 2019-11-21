using System;
using System.Collections.Generic;
using ConferenceApp.API.Interfaces;
using ConferenceApp.API.ViewModels;
using ConferenceApp.Core.Extensions;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Models;

namespace ConferenceApp.API.Repositories
{
    public class ReportRepositoryAdapter : IReportRepositoryAdapter
    {
        private readonly IReportRepository _reportRepository;


        public ReportRepositoryAdapter(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }


        public void Insert( ReportViewModel model )
        {
            var reportModel = new ReportModel
            {
                RequestId     = model.RequestId,
                Title         = model.Title,
                ReportType    = model.ReportType,
                File          = model.File.ConvertToFileStream(),
                Collaborators = model.Collaborators
            };
            
            _reportRepository.Insert(reportModel);
        }

        
        public void Update(ReportViewModel model)
        {
            var reportModel = new ReportModel
            {
                ReportId     = model.ReportId,
                Title        = model.Title,
                ReportType   = model.ReportType,
                File         = model.File.ConvertToFileStream(),
                ReportStatus = model.ReportStatus
            };
            _reportRepository.Update( reportModel );
        }
        

        public void Delete( Guid reportId )
        {
            _reportRepository.Get(reportId);
        }


        public ReportViewModel Get( Guid reportId )
        {
            var report = _reportRepository.Get(reportId);
            var model = new ReportViewModel
            {
                Title = report.Title,
                ReportId = report.ReportId,
                RequestId = report.RequestId,
                ReportType = report.ReportType,
                ReportStatus = report.ReportStatus
            };
            return model;
        }


        public IEnumerable<ReportViewModel> Get( Func<ReportViewModel, bool> filter )
        {
            throw new NotImplementedException();
        }


        public IEnumerable<ReportViewModel> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}