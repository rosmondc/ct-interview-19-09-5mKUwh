using System;
using System.Collections.Generic;

namespace Ct.Interview.Web.Api.Models
{
    public partial class AsxListedCompany
    {
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public string AsxCode { get; set; }
        public string GicsIndustryGroup { get; set; }
    }
}
