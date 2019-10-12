using AutoMapper;
using Ct.Interview.Data.Models;
using Ct.Interview.Web.Api.ViewModels;

namespace Ct.Interview.Web.Api.Mappings
{
    /// <summary>  
    /// Map entity object to ViewModel
    /// </summary>  
    public class AsxCompanyProfile : Profile
    {
        public AsxCompanyProfile()
        {
            CreateMap<AsxListedCompany, AsxListedCompanyResponse>();
        }
    }
}
