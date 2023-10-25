using MultiTenantTest.Dominio.Entidades;

namespace MultiTenantTest.Infraestructura.Repository
{
    public interface IVinculationRepository
    {
        Task Insert(Vinculation vinculation);
        IQueryable<Vinculation> GetByUserId(string userId);
    }
}
