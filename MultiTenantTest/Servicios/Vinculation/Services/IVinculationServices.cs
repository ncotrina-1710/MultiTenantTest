using MultiTenantTest.Servicios.Vinculation.Dtos;

namespace MultiTenantTest.Servicios.Vinculation.Services
{
    public interface IVinculationServices
    {
        Task Insert(VinculationDto vinculation);
        IEnumerable<VinculationDto> GetByUsuario(string userId);
    }
}
