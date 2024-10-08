﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto
{
    public class VillaUpdateDTO
	{
		[Required]

		public int Id { get; set; }
        [Required]
        public string Name { get; set; }
		

		public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
		[Required]

		public int Sqft { get; set; }
		[Required]

		public int Occupancy { get; set; }


        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        public IFormFile? Image { get; set; }
        public string Amenity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
