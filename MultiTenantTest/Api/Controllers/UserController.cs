using Microsoft.AspNetCore.Mvc;
using MultiTenantTest.Servicios.User.DTOs;
using MultiTenantTest.Servicios.User.Services;

namespace MultiTenantTest.Api.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(CredencialesUsuario credencialesUsuario)
        {
            var result = await _userServices.Insert(credencialesUsuario);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(CredencialesUsuario credencialesUsuario)
        {
            var result = await _userServices.Login(credencialesUsuario);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}