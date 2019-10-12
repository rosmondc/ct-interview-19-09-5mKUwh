using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ct.Interview.Repository.Interfaces
{
    public interface IRepository<T> where T: class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int Id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
