using Microsoft.AspNetCore.Mvc;
using MultiTenantTest.Servicios.Organization.Dtos;
using MultiTenantTest.Servicios.Organization.Services;

namespace MultiTenantTest.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationServices _organizationServices;
        public OrganizationController(IOrganizationServices organizationServices)
        {
            _organizationServices = organizationServices;
        }

        [HttpPost]
        public async Task<IActionResult> Insert(OrganizationDto organization)
        {
            var result = await _organizationServices.Insert(organization);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _organizationServices.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}