using Microsoft.EntityFrameworkCore;
using MultiTenantTest.Dominio.Entidades;
using MultiTenantTest.Infraestructura.Admin;
using MultiTenantTest.Infraestructura.Productos;

namespace MultiTenantTest.Infraestructura.Repository
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ApiDbContext _context;

        public OrganizationRepository(ApiDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<string> GetCadenaConexion(string tenantId)
        {
            var tenant = await _context.Organizations.FirstOrDefaultAsync(t => t.SlugTenant == tenantId);
            if (tenant != null)
                return tenant.CadenaConexion;

            return string.Empty;
        }
        public async Task<IEnumerable<Organization>> GetAll()
        {
            var organizations = await _context.Organizations.ToListAsync();
            return organizations;
        }

        public async Task<Organization> Get(int id)
        {
            return await _context.Organizations.FirstOrDefaultAsync(t => t.OrganizationId == id);
        }

        public async Task<bool> ValidateDuplicate(string slugTenant)
        {
            return await _context.Organizations.AnyAsync(x => x.SlugTenant.ToLower() == slugTenant.ToLower());
        }

        /// <summary>
        /// Metodo que inserta una Organización, además crea ka BD de la organización
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public async Task Insert(Organization organization)
        {

            await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();

            //Proceso para crear la BD de la organización
            var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>()
          .UseSqlServer(organization.CadenaConexion)
          .Options;
            using (var context = new ProductDbContext(optionsBuilder))
            {
                context.Database.Migrate();
            }
        }

    }
}
