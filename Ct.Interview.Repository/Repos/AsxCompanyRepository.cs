using Ct.Interview.Data.Models;
using Ct.Interview.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ct.Interview.Repository.Repos
{
    public class AsxCompanyRepository : GenericRepository<AsxListedCompany>, IAsxCompanyRepository
    {
        private readonly CtInterviewDBContext _context;
        private readonly ILogger<AsxCompanyRepository> _logger;
        public AsxCompanyRepository(CtInterviewDBContext context, ILogger<AsxCompanyRepository> logger) : base(context, logger)
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<AsxListedCompany> GetByCode(string code)
        {
            if (code.Length == 0)
                return null;

            try
            {
                _logger.LogInformation($"Start searching by code: {code}");
                return await this._context.AsxListedCompany.Where(x => x.AsxCode == code).FirstAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error {ex.Message} {ex.InnerException} Search by code: {code}");
                return null;
            }
        }
    }
}
