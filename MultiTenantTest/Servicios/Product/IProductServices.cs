using MultiTenantTest.Servicios.Common;
using MultiTenantTest.Servicios.Product.Dtos;

namespace MultiTenantTest.Servicios.Product
{
    public interface IProductServices
    {
        Task<ServiceResponse<IEnumerable<ProductDto>>> GetProducts(string tenantId);
        Task<ServiceResponse<bool>> Insert(ProductDto product, string tenantId);
        Task<ServiceResponse<bool>> Update(ProductDto product, string tenantId);
    }
}
