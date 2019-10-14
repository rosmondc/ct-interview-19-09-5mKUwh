using System.Threading.Tasks;

namespace Ct.Interview.Repository.Interfaces
{
    public interface ICsvHandler
    {
        bool DownloadAsxCompanyFile(string csvUrl, string filepath);
        Task ExportToSqlDatabase(string csvFilePath);
    }
}
