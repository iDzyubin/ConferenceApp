using System;
using System.IO;

namespace ConferenceApp.Core.Models
{
    public class ReportFile
    {
        public Guid ReportId { get; set; }
        public MemoryStream File { get; set; }
    }
}