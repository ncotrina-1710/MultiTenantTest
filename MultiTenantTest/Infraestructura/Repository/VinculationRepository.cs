using Microsoft.EntityFrameworkCore;
using MultiTenantTest.Dominio.Entidades;
using MultiTenantTest.Infraestructura.Admin;

namespace MultiTenantTest.Infraestructura.Repository
{
    public class VinculationRepository : IVinculationRepository
    {
        private readonly ApiDbContext _context;
        public VinculationRepository(ApiDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task Insert(Vinculation vinculation)
        {
            await _context.Vinculations.AddAsync(vinculation);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Vinculation> GetByUserId(string userId)
        {
            return _context.Vinculations.Include(x => x.Organization)
                                    .Where(x => x.UserId == userId)
                                    .AsQueryable();
        }

    }
}
