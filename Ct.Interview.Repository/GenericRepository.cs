using Ct.Interview.Data.Models;
using Ct.Interview.Repository.Interfaces;
using Ct.Interview.Repository.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ct.Interview.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly CtInterviewDBContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly ILogger<AsxCompanyRepository> _logger;

        /// <summary>  
        /// Initializes a new instance of the <see cref="GenericRepository{TEntity}"/> class.                  
        /// </summary>  
        public GenericRepository(CtInterviewDBContext context, ILogger<AsxCompanyRepository> logger)
        {
            this._context = context;
            this._dbSet = this._context.Set<T>();
            this._logger = logger;
        }

        public void Add(T entity)
        {
            try
            {
                _logger.LogInformation($"Start adding new data.");
                _dbSet.Add(entity);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error {ex.Message} {ex.InnerException} Data: {entity}");
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                _logger.LogInformation($"Start to get all records.");
                return await this._dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error {ex.Message} {ex.InnerException}");
                return null;
            }
        }

        public async Task<T> GetById(long id)
        {
            try
            {
                _logger.LogInformation($"Start searching by id: {id}");
                return await this._dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error {ex.Message} {ex.InnerException} Search by Id: {id}");
                return null;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                _logger.LogInformation($"Start removing data.");
                this._dbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error {ex.Message} {ex.InnerException} Data: {entity}");
            }
        }

        public void Update(T entity)
        {
            try
            {
                _logger.LogInformation($"Start updating data.");
                this._dbSet.Attach(entity);
                this._context.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error {ex.Message} {ex.InnerException} Data: {entity}");
            }
        }

        public void AddRange(IEnumerable<T> entities)
        {
            try
            {
                _logger.LogInformation($"Start saving set of datas.");
                _dbSet.AddRange(entities);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error {ex.Message} {ex.InnerException}");    
            }
        }

        public void Truncate()
        {
            try
            {
                _logger.LogInformation($"Start truncating records.");
                this._context.Database.ExecuteSqlCommand((string)$"TRUNCATE TABLE {typeof(T).Name}");
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error {ex.Message} {ex.InnerException}");
            }
        }
    }
}
