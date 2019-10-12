using System;
using System.Collections.Generic;

namespace Ct.Interview.Data.Models
{
    public partial class AsxListedCompany
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string AsxCode { get; set; }
        public string GicsIndustryGroup { get; set; }
    }
}
