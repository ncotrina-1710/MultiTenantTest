using MultiTenantTest.Servicios.Common;
using MultiTenantTest.Servicios.Organization.Dtos;

namespace MultiTenantTest.Servicios.Organization.Services
{
    public interface IOrganizationServices
    {

        Task<string> GetCadenaConexion(string tenantId);
        Task<ServiceResponse<bool>> Insert(OrganizationDto organization);
        Task<OrganizationDto> Get(int organizationId);
        Task<ServiceResponse<IEnumerable<OrganizationDto>>> GetAll();
    }
}
