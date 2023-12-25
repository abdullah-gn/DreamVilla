﻿using System.ComponentModel.DataAnnotations;

namespace DreamVilla_VillaApi.Models.Dto
{
    public class VillaUpdateDTO
	{
		[Required]

		public int Id { get; set; }
        [Required]
        public string Name { get; set; }
		[Required]

		public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
		[Required]

		public int Sqft { get; set; }
		[Required]

		public int Occupancy { get; set; }
		[Required]

		public string ImageUrl { get; set; }
        public string Amenity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
