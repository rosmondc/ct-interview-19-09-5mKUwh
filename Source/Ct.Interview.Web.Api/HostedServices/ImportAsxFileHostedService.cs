using Ct.Interview.Data.Models;
using Ct.Interview.Repository;
using Ct.Interview.Repository.Interfaces;
using Ct.Interview.Web.Api.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ct.Interview.Web.Api.HostedServices
{
    public class ImportAsxFileHostedService : BackgroundService, IHostedService
    {
        public IServiceScopeFactory _serviceScopeFactory;

        static readonly HttpClient client = new HttpClient();
        private readonly IConfiguration _configuration;
        private readonly string _csvUrl;
        private readonly string _csvFilePath;

        public ImportAsxFileHostedService(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _configuration = configuration;
            _csvUrl = _configuration.GetValue<string>("AsxSettings:ListedSecuritiesCsvUrl");
            _csvFilePath = _configuration.GetValue<string>("FolderFiles:CsvPath");
            _serviceScopeFactory = serviceScopeFactory;
        }

        bool DownloadAsxCompanyFile()
        {
            try
            {
                var wc = new WebClient();
                wc.DownloadFile(this._csvUrl, this._csvFilePath);

                return true;
            }
            catch (HttpRequestException e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        async Task ExportToSqlDatabase()
        {
            try
            {
                var companies = File.ReadAllLines(this._csvFilePath)
                                          .Skip(3)
                                          .Select(v => AsxCompanyValues.FromCsv(v))
                                          .ToList();

                if (companies.Count > 0)
                {
                    using (var unitOfWork = new UnitOfWork(new CtInterviewDBContext()))
                    {
                        var asxListedCompanyRange = new List<AsxListedCompany>();
                        var asxListedCompanies = new List<AsxListedCompany>();

                        foreach (var companyValue in companies)
                        {
                            var company = new AsxListedCompany();
                            company.AsxCode = companyValue.AsxCode;
                            company.CompanyName = companyValue.companyName;
                            company.GicsIndustryGroup = companyValue.GicsIndustryGroup;
                            asxListedCompanies.Add(company);
                        }
                        asxListedCompanies.AddRange(asxListedCompanies);

                        //Truncate table first before reseeding
                        unitOfWork.AsxCompanyRepository.Truncate();

                        unitOfWork.AsxCompanyRepository.AddRange(asxListedCompanies);
                        await unitOfWork.Commit();
                    }

                    Debug.WriteLine($"Parse data. {companies}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
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
                    else
                    {
                        Debug.WriteLine($"Unable to download file.");
                    }

                    await Task.Delay(100000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
