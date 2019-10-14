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
        public async Task<ActionResult<AsxListedCompanyResponse>> GetById(long id)
        {
            var result = await this._uow.AsxCompanyRepository.GetById(id);
            if (result != null)
                return _mapper.Map<AsxListedCompanyResponse>(result);

            return NotFound();
        }

        [HttpGet]
        [Route("/GetByCode")]
        public async Task<ActionResult<AsxListedCompanyResponse>> GetByCode(string asxCode)
        {
            var result = await this._uow.AsxCompanyRepository.GetByCode(asxCode);
            if (result != null)
                return _mapper.Map<AsxListedCompanyResponse>(result);

            return NotFound();
        }

        [HttpGet]
        [Route("/GetAll")]
        public async Task<ActionResult<AsxListedCompanyResponse[]>> GetAll()
        {
            var results = await this._uow.AsxCompanyRepository.GetAll();
            if (results != null)
                return _mapper.Map<AsxListedCompanyResponse[]>(results);
            return NotFound();
        }
    }
}
