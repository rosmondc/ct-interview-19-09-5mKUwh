using System.Threading.Tasks;

namespace Ct.Interview.Repository.Interfaces
{
    public interface ICsvHandler
    {
        bool DownloadAsxCompanyFile();
        Task ExportToSqlDatabase();
    }
}
