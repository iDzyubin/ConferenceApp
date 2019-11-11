using System.Collections.Generic;
using System.Linq;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.API.Extensions
{
    public static class UserExtensions
    {
        public static IEnumerable<Collaborator> ConvertToCollaborators( this IEnumerable<User> users )
        {
            var collaborators = from user in users
                select new Collaborator
                {
                    UserId = user.Id
                };

            return collaborators.AsEnumerable();
        }
    }
}