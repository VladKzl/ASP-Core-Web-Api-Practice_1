using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies")]
    [ApiExplorerSettings(GroupName = "v2")]
    [ApiController]
    public class CompaniesV2Controller : ControllerBase
    {
        public CompaniesV2Controller(IServiceManager service) => _service = service;

        private readonly IServiceManager _service;

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _service.CompanyService.GetAllCompaniesAsync(trackChanges: false);
            var companiesV2 = companies.Select(x => $"{x.Name} V2");
            return Ok(companiesV2);
        }
    }
}
