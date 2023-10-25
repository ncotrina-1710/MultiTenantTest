using MultiTenantTest.Servicios.Organization.Dtos;

namespace MultiTenantTest.Servicios.Vinculation.Dtos
{
    public class VinculationDto
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public OrganizationDto Organization { get; set; }
    }
}
