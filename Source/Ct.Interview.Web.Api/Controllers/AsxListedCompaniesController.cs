using System.Threading.Tasks;
using AutoMapper;
using Ct.Interview.Repository;
using Ct.Interview.Repository.Interfaces;
using Ct.Interview.Web.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ct.Interview.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsxListedCompaniesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AsxListedCompaniesController> _logger;
        private readonly UnitOfWork _uow;
        public AsxListedCompaniesController(IUnitOfWork uow, IMapper mapper, ILogger<AsxListedCompaniesController> logger)
        {
            this._uow = uow as UnitOfWork;
            this._mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("/GetById")]
        [ResponseCache(VaryByHeader = "GetById", Duration = 1000)]
        public async Task<ActionResult<AsxListedCompanyResponse>> GetById(long id)
        {
            _logger.LogInformation($"Execute endpoint id : {id}");
            var result = await this._uow.AsxCompanyRepository.GetById(id);
            if (result != null)
                return _mapper.Map<AsxListedCompanyResponse>(result);

            return NotFound();
        }

        [HttpGet]
        [Route("/GetByCode")]
        [ResponseCache(VaryByHeader = "GetByCode", Duration = 1000)]
        public async Task<ActionResult<AsxListedCompanyResponse>> GetByCode(string asxCode)
        {
            _logger.LogInformation($"Execute endpoint code: {asxCode}");

            var result = await this._uow.AsxCompanyRepository.GetByCode(asxCode);
            if (result != null)
                return _mapper.Map<AsxListedCompanyResponse>(result);

            return NotFound();
        }

        [HttpGet]
        [Route("/GetAll")]
        [ResponseCache(VaryByHeader = "AsxList", Duration = 1000)]
        public async Task<ActionResult<AsxListedCompanyResponse[]>> GetAll()
        {
            _logger.LogInformation("Execute endpoint");
            var results = await this._uow.AsxCompanyRepository.GetAll();
            if (results != null)
                return _mapper.Map<AsxListedCompanyResponse[]>(results);
            return NotFound();
        }
    }
}
