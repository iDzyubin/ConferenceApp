using System;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Web.ViewModels
{
    public class UpdateUserViewModel
    {
        public Guid      Id                  { get; set; } 
        public string    FirstName           { get; set; } 
        public string    MiddleName          { get; set; } 
        public string    LastName            { get; set; } 
        public string    Position            { get; set; } 
        public string    Organisation        { get; set; } 
        public string    City                { get; set; }
        public string    OrganisationAddress { get; set; } 
        public string    Phone               { get; set; } 
        public DateTime? StartResidenceDate  { get; set; } 
        public DateTime? EndResidenceDate    { get; set; }
        public UserRole  UserRole            { get; set; }
    }
}