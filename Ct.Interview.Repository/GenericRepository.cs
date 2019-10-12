using Ct.Interview.Data.Models;
using Ct.Interview.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ct.Interview.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly CtInterviewDBContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(CtInterviewDBContext context)
        {
            this._context = context;
            this._dbSet = this._context.Set<T>();
        }

        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await this._dbSet.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await this._dbSet.FindAsync(id);
        }

        public void Delete(T entity)
        {
            this._dbSet.Remove(entity);
        }

        public void Update(T entity)
        {
            this._dbSet.Attach(entity);
            this._context.Entry(entity).State = EntityState.Modified;
        }
    }
}
