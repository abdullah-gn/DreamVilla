using Microsoft.AspNetCore.Identity;

namespace DreamVilla_VillaApi.Models.Dto
{
    public class AppUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }

        public string Errors { get; set; }
    }
}
