using System;
using System.Threading.Tasks;

namespace Ct.Interview.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task Commit();
    }
}
