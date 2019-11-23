using ConferenceApp.API.ViewModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.API.Extensions
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