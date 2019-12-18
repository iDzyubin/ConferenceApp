using System;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Web.Models;

namespace ConferenceApp.Web.ViewModels
{
    public class TokenViewModel
    {
        public JsonWebToken JsonWebToken { get; set; }
        public Guid UserId { get; set; }
        public UserRole Role { get; set; }
    }
}