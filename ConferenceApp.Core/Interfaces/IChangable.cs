using System;
using System.Threading.Tasks;

namespace ConferenceApp.Core.Interfaces
{
    public interface IChangable<T> where T : Enum
    {
        Task ChangeStatusAsync( Guid reportId, T to );
    }
}