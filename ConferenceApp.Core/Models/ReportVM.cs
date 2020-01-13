using System;
using System.Collections.Generic;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Models
{
    /// <summary>
    /// TODO. Данная модель - экстренное решение проблем с маппингом.
    /// TODO. При первой же возможности решить проблему с маппингом
    /// TODO. и найти более адекватное решение для архитектуры репозитория репортов.
    /// </summary>
    public class ReportVM
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        
        public string Title { get; set; }
        
        public ReportType ReportType { get; set; }
        
        public ReportStatus ReportStatus { get; set; }
        
        public List<Collaborator> Collaborators { get; set; }
        
        public Guid SectionId { get; set; }
        
        public string FileName { get; set; }
    }
}