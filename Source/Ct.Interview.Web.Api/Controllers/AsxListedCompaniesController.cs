using System.Threading.Tasks;
using AutoMapper;
using Ct.Interview.Repository;
using Ct.Interview.Repository.Interfaces;
using Ct.Interview.Web.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Ct.Interview.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsxListedCompaniesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _uow;
        public AsxListedCompaniesController(IUnitOfWork uow, IMapper mapper)
        {
            this._uow = uow as UnitOfWork;
            this._mapper = mapper;
        }

        [HttpGet]
        [Route("/GetById")]
        [ResponseCache(VaryByHeader = "AsxList", Duration = 1000)]
        public async Task<ActionResult<AsxListedCompanyResponse>> GetById(int id)
        {
            return _mapper.Map<AsxListedCompanyResponse>(await this._uow.AsxCompanyRepository.GetById(id));
        }

        [HttpGet]
        [Route("/GetByCode")]
        [ResponseCache(VaryByHeader = "AsxList", Duration = 1000)]
        public async Task<ActionResult<AsxListedCompanyResponse>> GetByCode(string asxCode)
        {
            return _mapper.Map<AsxListedCompanyResponse>(await this._uow.AsxCompanyRepository.GetByCode(asxCode));
        }

        [HttpGet]
        [Route("/GetAll")]
        [ResponseCache(VaryByHeader = "AsxList", Duration = 1000)]
        public async Task<ActionResult<AsxListedCompanyResponse[]>> GetAll()
        {
            return _mapper.Map<AsxListedCompanyResponse[]>(await this._uow.AsxCompanyRepository.GetAll());
        }
    }
}
