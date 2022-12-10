using API.Core.DTOs.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.API.Bases;
using Web.API.Repositories.Interfaces;

namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ApiControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        public CompanyController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var companies = await _companyRepository.GetAll();
            return Ok(companies);
        }
    }
}
