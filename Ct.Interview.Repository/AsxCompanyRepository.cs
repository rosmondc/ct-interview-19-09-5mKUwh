using Ct.Interview.Data.Models;
using Ct.Interview.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Ct.Interview.Repository
{
    public class AsxCompanyRepository : GenericRepository<AsxListedCompany>, IAsxCompanyRepository
    {
        private readonly CtInterviewDBContext _context;
        public AsxCompanyRepository(CtInterviewDBContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<AsxListedCompany> GetByCode(string code)
        {
            return await this._context.AsxListedCompany.Where(x => x.AsxCode == code).FirstAsync();
        }

    }
}
