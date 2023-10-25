using Microsoft.AspNetCore.Identity;

namespace MultiTenantTest.Dominio.Entidades
{
    public class Vinculation
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string UserId { get; set; } = null!;
        public Organization Organization { get; set; } = null!;
        public IdentityUser User { get; set; } = null!;
    }
}
