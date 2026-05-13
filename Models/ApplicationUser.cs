using Microsoft.AspNetCore.Identity;

namespace Kaizen.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; }
    }

}
