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
        public double Rate { get; set; }
		public int Occupancy { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        public string Amenity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
