using AutoMapper;
using Ct.Interview.Repository.FileHandlers;
using Ct.Interview.Repository.Repos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Ct.Interview.Tests.Repository.FileHandlers
{
    public class CsvHandlerTests
    {
        [Theory]
        [InlineData("https://www.asx.com.au/asx/research/ASXListedCompanies.csv", "..\\..\\..\\logs\\AsxCompanyTest.csv")]        
        public void DownloadAsxCompanyCsvFile_CsvUrlAndFilePathShouldBeValid(string csvUrl, string filepath)
        {
            var _logger = new Mock<ILogger<AsxCompanyRepository>>();
            var _mapper = new Mock<IMapper>();

            var handler = new CsvHandler(_logger.Object, _mapper.Object);

            var actual = handler.DownloadAsxCompanyFile(csvUrl, filepath);
            Assert.True(actual);

        }

        [Theory]
        [InlineData("", "")]
        [InlineData("https://www.asx.com.au/asx/research/ASXListedCompanies.csv", "")]        
        public void GetByCode_CsvUrlAndFileShouldBeInvalid(string csvUrl, string filepath)
        {
            var _logger = new Mock<ILogger<AsxCompanyRepository>>();
            var _mapper = new Mock<IMapper>();

            var handler = new CsvHandler(_logger.Object, _mapper.Object);

            var actual = handler.DownloadAsxCompanyFile(csvUrl, filepath);
            Assert.False(actual);
        }

    }
}
