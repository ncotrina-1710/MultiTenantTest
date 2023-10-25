using System.ComponentModel.DataAnnotations;

namespace MultiTenantTest.Servicios.User.DTOs
{
    public class CredencialesUsuario
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int OrganizationId { get; set; }
    }
}
