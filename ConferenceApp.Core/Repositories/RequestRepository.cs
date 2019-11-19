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
            => _db = db;
        
        
        public void Insert( Request request )
        {
            request.Id = Guid.NewGuid();
            _db.Insert( request );
        }

        
        public void Delete( Guid userId )
            => _db.Requests.Delete( x => x.Id == userId );


        public void Update( Request request )
            => _db.Update( request );
        

        public Request Get( Guid userId )
            => _db.Requests.FirstOrDefault( x => x.Id == userId );

        
        public IEnumerable<Request> Get( Func<Request, bool> filter )
            => _db.Requests.Where( filter ).AsEnumerable();

        
        public IEnumerable<Request> GetAll()
            => _db.Requests.AsEnumerable();
    }
}