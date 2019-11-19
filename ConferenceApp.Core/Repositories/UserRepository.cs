using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using LinqToDB;

namespace ConferenceApp.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MainDb _db;


        public UserRepository( MainDb db )
            => _db = db;
        
        
        public void Insert( User user )
            => InsertWithId( user );


        public void Delete( Guid userId )
            => _db.Users.Delete( x => x.Id == userId );


        public void Update( User user )
            => _db.Update( user );


        public User Get( Guid userId )
            => _db.Users.FirstOrDefault( x => x.Id == userId );

        
        public IEnumerable<User> Get( Func<User, bool> filter )
            => _db.Users.Where( filter ).AsEnumerable();


        public IEnumerable<User> GetAll()
            => _db.Users.AsEnumerable();

        
        public Guid InsertWithId( User user )
        {
            user.Id = Guid.NewGuid();
            _db.Insert( user );
            return user.Id;
        }
    }
}