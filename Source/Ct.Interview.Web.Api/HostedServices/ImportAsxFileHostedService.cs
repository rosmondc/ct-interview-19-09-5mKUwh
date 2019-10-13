using Ct.Interview.Data.Models;
using Ct.Interview.Repository;
using Ct.Interview.Common.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ct.Interview.Common.Models;
using Ct.Interview.Common.ViewModels;

namespace Ct.Interview.Web.Api.HostedServices
{
    public class ImportAsxFileHostedService : BackgroundService, IHostedService
    {
        public IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<AsxCompanyRepository> _logger;
        private readonly IMapper _mapper;
        static readonly HttpClient client = new HttpClient();
        private readonly IConfiguration _configuration;
        private readonly string _csvUrl;
        private readonly string _csvFilePath;
        private string _backgroundProcess;

        public ImportAsxFileHostedService(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory
            , ILogger<AsxCompanyRepository> logger, IMapper mapper)
        {
            _configuration = configuration;
            _csvUrl = _configuration.GetValue<string>("AsxSettings:ListedSecuritiesCsvUrl");
            _csvFilePath = _configuration.GetValue<string>("FolderFiles:CsvPath");
            _backgroundProcess = _configuration.GetValue<string>("BackgroundProcessScheduleInMilliseconds");
            _serviceScopeFactory = serviceScopeFactory;
            this._logger = logger;
            this._mapper = mapper;
        }

        bool DownloadAsxCompanyFile()
        {
            this._logger.LogInformation("Downloading CSV file.");
            var wc = new WebClient();
            wc.DownloadFile(this._csvUrl, this._csvFilePath);

            return true;
        }

        async Task ExportToSqlDatabase()
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

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {

                    if (DownloadAsxCompanyFile())
                    {
                        await ExportToSqlDatabase();
                    }

                    await Task.Delay(this._backgroundProcess.TimerScheduleConvertToInt(), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error when executing background process. { ex.Message } { ex.InnerException}");
                throw ex;
            }

        }
    }
}
