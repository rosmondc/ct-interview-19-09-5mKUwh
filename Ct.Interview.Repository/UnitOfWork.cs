using System.Threading.Tasks;
using Ct.Interview.Data.Models;
using Ct.Interview.Repository.Interfaces;

namespace Ct.Interview.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CtInterviewDBContext _context;
        public AsxCompanyRepository AsxCompanyRepository { get; private set; }

        public UnitOfWork(CtInterviewDBContext context)
        {
            this._context = context;
            this.AsxCompanyRepository = new AsxCompanyRepository(this._context);
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
