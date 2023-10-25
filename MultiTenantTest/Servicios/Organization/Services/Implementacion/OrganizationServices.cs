using AutoMapper;
using MultiTenantTest.Dominio.Entidades;
using MultiTenantTest.Infraestructura.Repository;
using MultiTenantTest.Servicios.Common;
using MultiTenantTest.Servicios.Organization.Dtos;
using MultiTenantTest.Servicios.Product.Dtos;

namespace MultiTenantTest.Servicios.Organization.Services.Implementacion
{
    public class OrganizationServices : IOrganizationServices
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IMapper _mapper;
        public IConfiguration _configuration { get; }
        public OrganizationServices(IOrganizationRepository organizationRepository, IMapper mapper, IConfiguration configuration)
        {
            _organizationRepository = organizationRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<string> GetCadenaConexion(string tenantId)
        {
            return await _organizationRepository.GetCadenaConexion(tenantId);
        }

        public async Task<ServiceResponse<bool>> Insert(OrganizationDto organization)
        {
            var result = new ServiceResponse<bool>();
            var cadenaConexion = _configuration.GetConnectionString("Productos");
            if (string.IsNullOrWhiteSpace(cadenaConexion))
            {
                result.Message = "No existe cadena de conexíón para crear la BD de la organización";
                return result;
            }

            var isDuplicate = await _organizationRepository.ValidateDuplicate(organization.SlugTenant.Trim());
            if (isDuplicate)
            {
                result.Message = $"Ya existe una organización con el valor SlugTenant: {organization.SlugTenant.Trim().ToLower()}";
                return result;
            }

            organization.CadenaConexion = string.Format(cadenaConexion, organization.SlugTenant.Trim());
            var dato = _mapper.Map<Dominio.Entidades.Organization>(organization);

            await _organizationRepository.Insert(dato);
            result.Success = true;
            result.Data = true;
            return result;
        }

        public async Task<OrganizationDto> Get(int organizationId)
        {
            var organization = await _organizationRepository.Get(organizationId);
            var dato = _mapper.Map<OrganizationDto>(organization);
            return dato;
        }

        public async Task<ServiceResponse<IEnumerable<OrganizationDto>>> GetAll()
        {
            var result = new ServiceResponse<IEnumerable<OrganizationDto>>();
            var organizations = await _organizationRepository.GetAll();
            var resultData = _mapper.Map<IEnumerable<OrganizationDto>>(organizations);
            result.Data = resultData;
            result.Success = true;
            return result;
        }
    }
}
