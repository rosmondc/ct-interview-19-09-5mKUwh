using AutoMapper;
using Ct.Interview.Data.Models;
using Ct.Interview.Web.Api.ViewModels;

namespace Ct.Interview.Web.Api.Mapping
{
    public class AsxCompanyProfile : Profile
    {
        public AsxCompanyProfile()
        {
            CreateMap<AsxListedCompany, AsxListedCompanyResponse>();
        }
    }
}
