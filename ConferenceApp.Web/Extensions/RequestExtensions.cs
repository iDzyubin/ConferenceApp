using ConferenceApp.Core.Models;
using ConferenceApp.Web.ViewModels;

namespace ConferenceApp.Web.Extensions
{
    public static class RequestExtensions
    {
        public static RequestViewModel ConvertToRequestViewModel( this RequestModel request )
            => new RequestViewModel
            {
                User = request.User,
                Reports = request.Reports
            };


        public static RequestModel ConvertToRequestModel( this RequestViewModel model )
            => new RequestModel
            {
                User = model.User,
                Reports = model.Reports
            };
    }
}