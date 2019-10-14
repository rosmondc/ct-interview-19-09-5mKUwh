using AutoMapper;
using Ct.Interview.Common.Helpers;
using Ct.Interview.Common.ViewModels;
using Ct.Interview.Data.Models;
using Ct.Interview.Repository.Interfaces;
using Ct.Interview.Repository.Repos;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace Ct.Interview.Repository.FileHandlers
{
    public class CsvHandler : ICsvHandler
    {
        private ILogger<AsxCompanyRepository> _logger;
        private IMapper _mapper;

        public CsvHandler(ILogger<AsxCompanyRepository> logger, IMapper mapper)
        {
            this._logger = logger;
            this._mapper = mapper;
        }

        public bool DownloadAsxCompanyFile(string csvUrl, string filepath)
        {
            this._logger.LogInformation("Downloading CSV file.");
            var wc = new WebClient();

            if (csvUrl.Length == 0)
                return false;

            if (filepath.Length == 0)
                return false;

            wc.DownloadFile(csvUrl, filepath);

            return true;
        }

        public async Task ExportToSqlDatabase(string csvFilePath)
        {            
            this._logger.LogInformation("Start exporting data to the database.");
            var asxCompanies = this._mapper.Map<AsxListedCompany[]>(CsvParserHelper.ParseCsv<AsxCompanyViewModel>(csvFilePath));
            using (var unitOfWork = new UnitOfWork(new CtInterviewDBContext(), this._logger))
            {
                //Truncate table first before reseeding
                unitOfWork.AsxCompanyRepository.Truncate();

                unitOfWork.AsxCompanyRepository.AddRange(asxCompanies);
                await unitOfWork.Commit();
                this._logger.LogInformation("Data succesfully updated.");
            }
        }

    }
}
