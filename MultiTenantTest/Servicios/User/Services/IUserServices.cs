using MultiTenantTest.Servicios.Common;
using MultiTenantTest.Servicios.User.DTOs;

namespace MultiTenantTest.Servicios.User.Services
{
    public interface IUserServices
    {
        Task<ServiceResponse<bool>> Insert(CredencialesUsuario credencialesUsuario);
        Task<ServiceResponse<RespuestaAutenticacion>> Login(CredencialesUsuario credencialesUsuario);
    }
}
