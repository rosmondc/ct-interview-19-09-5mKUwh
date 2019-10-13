using CsvHelper;
using System.IO;
using System.Linq;

namespace Ct.Interview.Common.Helpers
{
    public class CsvParserHelper
    {
        /// <summary>
        /// DTO to convert CVS data to AsxCompanyViewModel
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// 
        public static T[] ParseCsv<T>(string filePath)
        {
            using (TextReader fileReader = File.OpenText(filePath))
            {
                var csv = new CsvReader(fileReader);
                csv.Configuration.HasHeaderRecord = false;
                csv.Read();
                csv.Read();
                return csv.GetRecords<T>().ToArray();
            }
        }
    }
}
