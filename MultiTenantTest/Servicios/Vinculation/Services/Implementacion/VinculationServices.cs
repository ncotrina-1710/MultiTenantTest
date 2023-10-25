using AutoMapper;
using MultiTenantTest.Infraestructura.Repository;
using MultiTenantTest.Servicios.Vinculation.Dtos;

namespace MultiTenantTest.Servicios.Vinculation.Services.Implementacion
{
    public class VinculationServices : IVinculationServices
    {
        private readonly IVinculationRepository _vinculationRepository;
        private readonly IMapper _mapper;
        public VinculationServices(IVinculationRepository vinculationRepository, IMapper mapper)
        {
            _vinculationRepository = vinculationRepository;
            _mapper = mapper;
        }

        public async Task Insert(VinculationDto vinculation)
        {
            var dato = _mapper.Map<Dominio.Entidades.Vinculation>(vinculation);
            await _vinculationRepository.Insert(dato);
        }
        public IEnumerable<VinculationDto> GetByUsuario(string userId)
        {
            var resultVinculacion = _vinculationRepository.GetByUserId(userId);
            var result = _mapper.Map<IEnumerable<VinculationDto>>(resultVinculacion);
            return result;
        }
    }
}
