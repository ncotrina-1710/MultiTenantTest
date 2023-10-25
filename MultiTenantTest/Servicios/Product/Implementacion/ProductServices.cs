using AutoMapper;
using MultiTenantTest.Infraestructura.Repository;
using MultiTenantTest.Servicios.Common;
using MultiTenantTest.Servicios.Organization.Services;
using MultiTenantTest.Servicios.Product.Dtos;

namespace MultiTenantTest.Servicios.Product.Implementacion
{
    public class ProductServices : IProductServices
    {

        private readonly IProductRepository _productRepository;
        private readonly IOrganizationServices _organizationServices;
        private readonly IMapper _mapper;
        public ProductServices(IProductRepository productRepository,
            IOrganizationServices organizationServices
            , IMapper mapper)
        {
            _productRepository = productRepository;
            _organizationServices = organizationServices;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<IEnumerable<ProductDto>>> GetProducts(string tenantId)
        {
            var result = new ServiceResponse<IEnumerable<ProductDto>>();
            var cadenaConexion = await GetCadenaConexion(tenantId);
            if (!cadenaConexion.Success)
            {
                result.Message = cadenaConexion.Message;
                return result;
            }

            var productos = await _productRepository.GetProducts(cadenaConexion.Data);
            var resultData = _mapper.Map<IEnumerable<ProductDto>>(productos);
            result.Data = resultData;
            result.Success = true;
            return result;
        }

        public async Task<ServiceResponse<bool>> Insert(ProductDto product, string tenantId)
        {
            var result = new ServiceResponse<bool>();
            var cadenaConexion = await GetCadenaConexion(tenantId);
            if (!cadenaConexion.Success)
            {
                result.Message = cadenaConexion.Message;
                return result;
            }

            var dato = _mapper.Map<Dominio.Entidades.Product>(product);
            await _productRepository.Insert(dato, cadenaConexion.Data);
            result.Success = true;
            result.Data = true;
            return result;
        }
        public async Task<ServiceResponse<bool>> Update(ProductDto product, string tenantId)
        {
            var result = new ServiceResponse<bool>();
            var cadenaConexion = await GetCadenaConexion(tenantId);
            if (!cadenaConexion.Success)
            {
                result.Message = cadenaConexion.Message;
                return result;
            }

            var dato = _mapper.Map<Dominio.Entidades.Product>(product);
            await _productRepository.Update(dato, cadenaConexion.Data);
            result.Success = true;
            result.Data = true;
            return result;
        }

        private async Task<ServiceResponse<string>> GetCadenaConexion(string tenantId)
        {
            var result = new ServiceResponse<string>();
            var connectionString = await _organizationServices.GetCadenaConexion(tenantId);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                result.Message = "Cadena de conexión es vacía";
                return result;
            }

            result.Success = true;
            result.Data = connectionString;
            return result;
        }
    }
}
