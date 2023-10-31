using System.ComponentModel.DataAnnotations;

namespace DreamVilla_VillaApi.Models.Dto
{
    public class VillaDTO
    {
        public int id { get; set; }
        [Required]
        [MaxLength(55)]
        public string name { get; set; }

        public int occupancy { get; set; }

        public int sqft { get; set; }
        
    }
}
