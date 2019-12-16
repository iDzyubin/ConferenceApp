using System;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Extensions
{
    public static class UserExtensions
    {
        public static User ConvertToUser(this UserModel model)
        {
            return new User
            {
                Id = model.Id,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Address = model.Address,
                Degree = model.Degree,
                Email = model.Email,
                Fax = model.Fax,
                Organization = model.Organization,
                Phone = model.Phone,
                EndResidenceDate = model.EndResidenceDate,
                StartResidenceDate = model.StartResidenceDate
            };
        }

        public static UserModel ConvertToUserModel(this User model)
        {
            return new UserModel
            {
                Id = model.Id,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Address = model.Address,
                Degree = model.Degree,
                Email = model.Email,
                Fax = model.Fax,
                Organization = model.Organization,
                Phone = model.Phone,
                EndResidenceDate = model.EndResidenceDate ?? new DateTime(),
                StartResidenceDate = model.StartResidenceDate ?? new DateTime()
            };
        }
    }
}