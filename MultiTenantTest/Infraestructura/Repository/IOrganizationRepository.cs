using MultiTenantTest.Dominio.Entidades;

namespace MultiTenantTest.Infraestructura.Repository
{
    public interface IOrganizationRepository
    {
        Task<string> GetCadenaConexion(string tenantId);
        Task Insert(Organization organization);
        Task<bool> ValidateDuplicate(string slugTenant);
        Task<IEnumerable<Organization>> GetAll();
        Task<Organization> Get(int id);
    }
}
