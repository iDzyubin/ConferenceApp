using System;
using System.Collections.Generic;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Interfaces
{
    public interface ICollaboratorRepository : IRepository<Collaborator>
    {
        void InsertRange( Guid reportId, List<Collaborator> collaborators );
        
        void DeleteByReport( Guid reportId );
    }
}