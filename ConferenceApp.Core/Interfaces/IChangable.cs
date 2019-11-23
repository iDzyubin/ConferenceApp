using System;

namespace ConferenceApp.Core.Interfaces
{
    public interface IChangable<T> where T : Enum
    {
        T ChangeStatusTo( Guid requestId, T status );
    }
}