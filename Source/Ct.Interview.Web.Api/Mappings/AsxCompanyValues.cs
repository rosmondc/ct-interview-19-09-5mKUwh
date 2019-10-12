
using Ct.Interview.Common.Helpers;
using System.Text.RegularExpressions;

namespace Ct.Interview.Web.Api.Mappings
{
    public class AsxCompanyValues
    {
        internal string companyName;
        internal string AsxCode;
        internal string GicsIndustryGroup;

        /// <summary>  
        /// Use Regex to handle commas within double quotes
        /// </summary>  
        public static AsxCompanyValues FromCsv(string csvLine)
        {
            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            string[] values = CSVParser.Split(csvLine);

            AsxCompanyValues company = new AsxCompanyValues();
            
            company.companyName = values[0].RemoveEscapeCharacter();
            company.AsxCode = values[1].RemoveEscapeCharacter();
            company.GicsIndustryGroup = values[2].RemoveEscapeCharacter();

            return company;
        }

    }
}
