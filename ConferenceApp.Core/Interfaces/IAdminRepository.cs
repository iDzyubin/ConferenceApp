using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Interfaces
{
    public interface IAdminRepository : IRepository<Admin>
    {
        Admin GetByEmail( string email );
    }
}