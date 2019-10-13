using System.Threading.Tasks;
using Ct.Interview.Data.Models;
using Ct.Interview.Repository.Interfaces;
using Ct.Interview.Repository.Repos;
using Microsoft.Extensions.Logging;

namespace Ct.Interview.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CtInterviewDBContext _context;
        public AsxCompanyRepository AsxCompanyRepository { get; private set; }


        public UnitOfWork(CtInterviewDBContext context, ILogger<AsxCompanyRepository> logger)
        {
            this._context = context;
            this.AsxCompanyRepository = new AsxCompanyRepository(this._context, logger);
        }

        public async Task Commit()
        {
            await this._context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}
