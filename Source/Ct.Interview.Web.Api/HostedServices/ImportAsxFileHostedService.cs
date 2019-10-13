using Ct.Interview.Common.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ct.Interview.Repository.Interfaces;
using Ct.Interview.Repository.Repos;

namespace Ct.Interview.Web.Api.HostedServices
{
    public class ImportAsxFileHostedService : BackgroundService, IHostedService
    {
        private readonly string _backgroundProcess;
        private readonly ILogger<AsxCompanyRepository> _logger;
        private readonly ICsvHandler _csvHandler;

        public ImportAsxFileHostedService(IConfiguration configuration, ILogger<AsxCompanyRepository> logger, ICsvHandler csvHandler)
        {
                this._backgroundProcess = configuration.GetValue<string>("BackgroundProcessScheduleInMilliseconds");
                this._logger = logger;
                this._csvHandler = csvHandler;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (this._csvHandler.DownloadAsxCompanyFile())
                    {
                        await this._csvHandler.ExportToSqlDatabase();
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
