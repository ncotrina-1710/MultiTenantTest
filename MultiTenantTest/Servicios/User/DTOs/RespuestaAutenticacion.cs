namespace MultiTenantTest.Servicios.User.DTOs
{
    public class RespuestaAutenticacion
    {
        public string Token { get; set; }
        public IEnumerable<Tenant> Tenants { get; set; }
    }

   
}
