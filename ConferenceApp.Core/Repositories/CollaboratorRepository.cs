using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using LinqToDB;

namespace ConferenceApp.Core.Repositories
{
    public class CollaboratorRepository : ICollaboratorRepository
    {
        private readonly MainDb _db;

        public CollaboratorRepository( MainDb db )
            => _db = db;

        public void Insert( Collaborator collaborator )
        {
            collaborator.Id = Guid.NewGuid();
            _db.Insert( collaborator );
        }

        public void InsertRange( Guid reportId, List<Collaborator> collaborators )
            => collaborators.ForEach( Insert );

        public void Delete( Guid id )
            => _db.Collaborators.Delete( x => x.Id == id );


        public void Update( Collaborator collaborator )
            => _db.Update( collaborator );


        public Collaborator Get( Guid id )
            => _db.Collaborators.FirstOrDefault( x => x.Id == id );


        public IEnumerable<Collaborator> GetAll()
            => _db.Collaborators.AsEnumerable();


        public void DeleteByReport( Guid reportId )
            => _db.Collaborators.Delete( x => x.ReportId == reportId );


        public IEnumerable<Collaborator> Get( Func<Collaborator, bool> filter )
            => throw new NotImplementedException();
    }
}