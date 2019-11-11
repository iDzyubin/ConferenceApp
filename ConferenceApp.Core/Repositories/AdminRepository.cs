using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using LinqToDB;

namespace ConferenceApp.Core.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly MainDb _db;

        public AdminRepository( MainDb db )
            => _db = db;
        
        
        public void Insert( Admin admin )
        {
            admin.Id    = Guid.NewGuid();
            admin.Login = admin.Login.ToLower();
            _db.Insert( admin );
        }

        
        public void Delete( Guid id )
            => _db.Admins.Delete( x => x.Id == id );

        
        public void Update( Admin admin )
            => _db.Update( admin );

        
        public Admin Get( Guid id )
            => _db.Admins.FirstOrDefault( x => x.Id == id );

        
        public IEnumerable<Admin> GetAll()
            => _db.Admins.AsEnumerable();


        public Admin GetByEmail( string email )
            => _db.Admins
                  .FirstOrDefault(x => x.Login.ToLower() == email.ToLower());
    }
}