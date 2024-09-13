using Microsoft.AspNetCore.Identity;

namespace DreamVilla_VillaApi.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
