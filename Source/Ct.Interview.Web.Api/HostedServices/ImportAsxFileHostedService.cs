using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ct.Interview.Repository.Interfaces;
using Ct.Interview.Repository.Repos;
using Polly;
using System.Net.Http;
using Polly.CircuitBreaker;
using Microsoft.Extensions.Caching.Memory;
using Ct.Interview.Common.Constants;

namespace Ct.Interview.Web.Api.HostedServices
{
    public class ImportAsxFileHostedService : BackgroundService, IHostedService
    {
        
        private readonly ILogger<AsxCompanyRepository> _logger;
        private readonly ICsvHandler _csvHandler;
        private readonly IMemoryCache _memoryCache;
        static string _csvUrl;
        static string _csvFilePath;
        private double _backgroundProcessRetry;
        private double _backgroundProcessScheduleInHours;

        public ImportAsxFileHostedService(IConfiguration configuration, ILogger<AsxCompanyRepository> logger, 
            ICsvHandler csvHandler, IMemoryCache memoryCache)
        {
            _backgroundProcessRetry = configuration.GetValue<double>("BackgroundProcessScheduleRetryInSeconds");
            _backgroundProcessScheduleInHours = configuration.GetValue<double>("BackgroundProcessScheduleInHours");
            _csvUrl = configuration.GetValue<string>("AsxSettings:WorkingCsvUrl");
            _csvFilePath = configuration.GetValue<string>("FolderFiles:CsvPath");
            this._logger = logger;
            this._csvHandler = csvHandler;
            this._memoryCache = memoryCache;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    bool greeting;
                    if (!_memoryCache.TryGetValue(CacheKeys.IsDataFoundOnCsvUrl, out greeting))
                    {
                        var isValidUrl = await ValidateCsvUrl();

                        if (isValidUrl)
                        {
                            if (this._csvHandler.DownloadAsxCompanyFile(_csvUrl, _csvFilePath))
                            {
                                await this._csvHandler.ExportToSqlDatabase(_csvFilePath);
                            }

                            _memoryCache.Set(CacheKeys.IsDataFoundOnCsvUrl, isValidUrl, new MemoryCacheEntryOptions()
                                .SetAbsoluteExpiration(TimeSpan.FromHours(this._backgroundProcessScheduleInHours)));
                        }
                    }
                   
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error when executing background process. { ex.Message } { ex.InnerException}");
                throw ex;
            }

        }


        #region Handle Retries

        static AsyncCircuitBreakerPolicy<HttpResponseMessage> breakerPolicy;
        static HttpClient httpClient = new HttpClient();

        private async Task<bool> ValidateCsvUrl()
        {
            breakerPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(this._backgroundProcessRetry),
                    OnBreak, OnReset, OnHalfOpen
                );

            HttpResponseMessage response = await breakerPolicy.ExecuteAsync(MakeHttpCall);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            this._logger.LogError($"Http status code { response.StatusCode }");
            return false;
        }

        static async Task<HttpResponseMessage> MakeHttpCall()
        {
            Console.WriteLine($"made http get callcsv url - braker state { breakerPolicy.CircuitState }");
            return await httpClient.GetAsync(_csvUrl);
        }

        static void OnHalfOpen()
        {
            Console.WriteLine("Half open");
            breakerPolicy.Reset();
        }

        static void OnReset(Context context) 
        {
            Console.WriteLine("Reset");
        }

        static void OnBreak(DelegateResult<HttpResponseMessage> delegateResult, TimeSpan timeSpan, Context context)
        {
            Console.WriteLine($"Break - break state{ breakerPolicy.CircuitState }");
        }

        #endregion

    }
}
