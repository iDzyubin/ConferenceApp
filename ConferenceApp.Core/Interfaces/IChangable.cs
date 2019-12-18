using System;

namespace ConferenceApp.Core.Interfaces
{
    public interface IChangable<T> where T : Enum
    {
        void ChangeStatus( Guid reportId, T to );
    }
}