using MultiTenantTest.Dominio.Entidades;

namespace MultiTenantTest.Infraestructura.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts(string cadenaConexion);
        Task Insert(Product product, string cadenaConexion);
        Task Update(Product product, string cadenaConexion);
    }
}
