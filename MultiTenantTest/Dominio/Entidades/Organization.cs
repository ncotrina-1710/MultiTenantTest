namespace MultiTenantTest.Dominio.Entidades
{
    public class Organization
    {
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string SlugTenant { get; set; }
        public string CadenaConexion { get; set; }
    }
}
