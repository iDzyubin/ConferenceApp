using System;
using System.Collections.Generic;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;

namespace ConferenceApp.Core.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        public void Insert( Request item )
        {
            throw new NotImplementedException();
        }

        public void Delete( Guid id )
        {
            throw new NotImplementedException();
        }

        public void Update( Request item )
        {
            throw new NotImplementedException();
        }

        public Request Get( Guid id )
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Request> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}