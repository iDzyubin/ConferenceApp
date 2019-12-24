using System;

namespace ConferenceApp.Web.ViewModels
{
    public class SignUpViewModel
    {
        public Guid      Id                  { get; set; } 
        public string    FirstName           { get; set; } 
        public string    MiddleName          { get; set; } 
        public string    LastName            { get; set; } 
        public string    Position            { get; set; } 
        public string    Organization        { get; set; } 
        public string    City                { get; set; }
        public string    OrganizationAddress { get; set; } 
        public string    Phone               { get; set; } 
        public string    Email               { get; set; } 
        public DateTime? StartResidenceDate  { get; set; } 
        public DateTime? EndResidenceDate    { get; set; }
        public string    Password            { get; set; }
    }
}