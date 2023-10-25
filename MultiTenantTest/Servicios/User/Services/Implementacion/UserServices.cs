using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MultiTenantTest.Servicios.Common;
using MultiTenantTest.Servicios.Organization.Services;
using MultiTenantTest.Servicios.User.DTOs;
using MultiTenantTest.Servicios.Vinculation.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MultiTenantTest.Servicios.User.Services.Implementacion
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IVinculationServices _vinculationServices;
        private readonly IOrganizationServices _organizationServices;
        private readonly SignInManager<IdentityUser> _signInManager;


        public UserServices(UserManager<IdentityUser> userManager, IConfiguration configuration,
            IVinculationServices vinculationServices, SignInManager<IdentityUser> signInManager,
            IOrganizationServices organizationServices)
        {
            _userManager = userManager;
            _configuration = configuration;
            _vinculationServices = vinculationServices;
            _signInManager = signInManager;
            _organizationServices = organizationServices;
        }


        public async Task<ServiceResponse<bool>> Insert(CredencialesUsuario credencialesUsuario)
        {
            var result = new ServiceResponse<bool>();

            var existOrganization = await _organizationServices.Get(credencialesUsuario.OrganizationId);
            if (existOrganization == null)
            {
                result.Message = $"No existe una organización con el Id: {credencialesUsuario.OrganizationId}";
                return result;
            }

            var usuario = new IdentityUser
            {
                UserName = credencialesUsuario.Email,
                Email = credencialesUsuario.Email
            };
            var resultado = await _userManager.CreateAsync(usuario, credencialesUsuario.Password);
            if (!resultado.Succeeded)
            {
                result.Message = string.Join(", ", resultado.Errors);
                return result;
            }

            var resultUsuario = await _userManager.FindByEmailAsync(credencialesUsuario.Email);
            if (resultUsuario == null)
            {
                result.Message = "No se encontró al usuario";
                return result;
            }

            await _vinculationServices.Insert(new Vinculation.Dtos.VinculationDto
            {
                UserId = resultUsuario.Id,
                OrganizationId = credencialesUsuario.OrganizationId
            });

            result.Success = true;
            result.Data = true;
            return result;

        }
        public async Task<ServiceResponse<RespuestaAutenticacion>> Login(CredencialesUsuario credencialesUsuario)
        {
            var result = new ServiceResponse<RespuestaAutenticacion>();
            var resultGetData = await _signInManager.PasswordSignInAsync(credencialesUsuario.Email,
                credencialesUsuario.Password, isPersistent: false, lockoutOnFailure: false);
            if (!resultGetData.Succeeded)
            {
                result.Message = "Login incorrecto";
                return result;
            }

            var tenantId = string.Empty;
            var usuario = await _userManager.FindByEmailAsync(credencialesUsuario.Email);
            var tenants = new List<Tenant>();
            if (usuario != null)
            {
                var resultVinculacion = _vinculationServices.GetByUsuario(usuario.Id);
                if (resultVinculacion.Any())
                {
                    tenants = resultVinculacion.Select(x => new Tenant { SlugTenant = x.Organization.SlugTenant }).ToList();
                    tenantId = tenants.FirstOrDefault()?.SlugTenant;
                }
            }

            var resultToken = await ConstruirToken(credencialesUsuario, tenantId);
            if (resultToken == null)
            {
                result.Message = "Ocurrió un error al crear el token";
                return result;
            }

            resultToken.Tenants = tenants;
            result.Data = resultToken;
            result.Success = true;
            return result;
        }


        private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credencialesUsuario, string tenantId)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credencialesUsuario.Email),
                new Claim("tenant", tenantId)
            };

            var usuario = await _userManager.FindByEmailAsync(credencialesUsuario.Email);
            var claimsDB = await _userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddMinutes(15);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken)
            };
        }
    }
}
