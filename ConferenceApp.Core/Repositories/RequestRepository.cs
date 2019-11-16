using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using LinqToDB;

namespace ConferenceApp.Core.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly MainDb _db;

        public RequestRepository( MainDb db )
        {
            _db = db;
        }
        
        public void Insert( Request item )
        {
            throw new NotImplementedException();
        }

        public void Delete( Guid id )
        {
            throw new NotImplementedException();
        }

        public void Update( Request request )
            => _db.Update( request );

        public Request Get( Guid id )
            => _db.Requests.FirstOrDefault( x => x.Id == id );

        public IEnumerable<Request> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}