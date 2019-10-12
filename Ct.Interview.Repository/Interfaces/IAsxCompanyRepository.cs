using Ct.Interview.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ct.Interview.Repository.Interfaces
{
    public interface IAsxCompanyRepository
    {
        Task<AsxListedCompany> GetByCode(string code);

    }
}
