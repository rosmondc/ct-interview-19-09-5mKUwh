using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ct.Interview.Repository.Interfaces
{
    public interface IRepository<T> where T: class
    {
        /// <summary>  
        /// Gets all.  
        /// </summary>  
        /// <returns></returns>  
        Task<IEnumerable<T>> GetAll();

        /// <summary>  
        /// Singles the or default.  
        /// </summary>  
        /// <param name="id">The entity id.</param>  
        /// <returns></returns>  
        Task<T> GetById(int Id);
        void Add(T entity);

        // <summary>  
        /// Adds the range.  
        /// </summary>  
        /// <param name="entities">The entities.</param>  
        void AddRange(IEnumerable<T> entities);

        /// <summary>  
        /// Update the Entity  
        /// </summary>  
        /// <param name="entityToUpdate"></param>  
        void Update(T entity);

        /// <summary>  
        /// Removes the Entity  
        /// </summary>  
        /// <param name="entityToDelete"></param> 
        void Delete(T entity);
    }
}
