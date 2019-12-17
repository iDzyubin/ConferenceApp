using System;

namespace ConferenceApp.Core.DataModels
{
    public partial class User
    {
        public UserRole Role { get; set; }
        
        public UserStatus Status { get; set; }
        
        public string Password { get; set; }
    }
}