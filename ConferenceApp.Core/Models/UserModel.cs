using System;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Models
{
    public class UserModel
    {
        public Guid      Id                 { get; set; } 
        public string    FirstName          { get; set; } 
        public string    MiddleName         { get; set; } 
        public string    LastName           { get; set; } 
        public Degree    Degree             { get; set; } 
        public string    Organization       { get; set; } 
        public string    Address            { get; set; } 
        public string    Phone              { get; set; } 
        public string    Fax                { get; set; } 
        public string    Email              { get; set; } 
        public DateTime StartResidenceDate  { get; set; } 
        public DateTime EndResidenceDate    { get; set; }
        public UserRole Role { get; set; }
    }
}