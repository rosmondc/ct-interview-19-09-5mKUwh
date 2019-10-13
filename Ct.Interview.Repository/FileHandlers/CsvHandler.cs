using AutoMapper;
using Ct.Interview.Common.Helpers;
using Ct.Interview.Common.ViewModels;
using Ct.Interview.Data.Models;
using Ct.Interview.Repository.Interfaces;
using Ct.Interview.Repository.Repos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace Ct.Interview.Repository.FileHandlers
{
    public class CsvHandler : ICsvHandler
    {
        private IConfiguration _configuration;
        private string _csvUrl;
        private string _csvFilePath;
        private string _backgroundProcess;
        private ILogger<AsxCompanyRepository> _logger;
        private IMapper _mapper;

        public CsvHandler(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory
            , ILogger<AsxCompanyRepository> logger, IMapper mapper)
        {
            this._configuration = configuration;
            this._csvUrl = _configuration.GetValue<string>("AsxSettings:ListedSecuritiesCsvUrl");
            this._csvFilePath = _configuration.GetValue<string>("FolderFiles:CsvPath");
            this._backgroundProcess = _configuration.GetValue<string>("BackgroundProcessScheduleInMilliseconds");
            this._logger = logger;
            this._mapper = mapper;
        }

        public bool DownloadAsxCompanyFile()
        {
            this._logger.LogInformation("Downloading CSV file.");
            var wc = new WebClient();
            wc.DownloadFile(this._csvUrl, this._csvFilePath);

            return true;
        }

        public async Task ExportToSqlDatabase()
        {
            this._logger.LogInformation("Start exporting data to the database.");
            var asxCompanies = this._mapper.Map<AsxListedCompany[]>(CsvParserHelper.ParseCsv<AsxCompanyViewModel>(this._csvFilePath));

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
