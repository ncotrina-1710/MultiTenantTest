using Microsoft.EntityFrameworkCore;
using MultiTenantTest.Dominio.Entidades;
using MultiTenantTest.Infraestructura.Productos;

namespace MultiTenantTest.Infraestructura.Repository
{
    public class ProductRepository : IProductRepository
    {
        public async Task<IEnumerable<Product>> GetProducts(string cadenaConexion)
        {
            var dataDbContext = GetDbContext(cadenaConexion);
            var products = await dataDbContext.Products.ToListAsync();
            return products;
        }

        public async Task Insert(Product product, string cadenaConexion)
        {
            var dataDbContext = GetDbContext(cadenaConexion);
            await dataDbContext.Products.AddAsync(product);
            await dataDbContext.SaveChangesAsync();
        }

        public async Task Update(Product product, string cadenaConexion)
        {
            var dataDbContext = GetDbContext(cadenaConexion);
            dataDbContext.Products.Update(product);
            await dataDbContext.SaveChangesAsync();
        }

        private ProductDbContext GetDbContext(string cadenaConexion)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>()
            .UseSqlServer(cadenaConexion)
            .Options;
            return new ProductDbContext(optionsBuilder);
        }

    }
}
