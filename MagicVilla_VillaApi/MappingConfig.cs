using AutoMapper;
using DreamVilla_VillaApi.Models;
using DreamVilla_VillaApi.Models.Dto;

namespace DreamVilla_VillaApi
{
	public class MappingConfig :Profile
	{
        public MappingConfig()
        {
			
			CreateMap<Villa,VillaDTO>().ReverseMap();
			CreateMap<VillaUpdateDTO, Villa>().ReverseMap();
			CreateMap<VillaCreateDTO, Villa>().ReverseMap();
			CreateMap<VillaNumber,VillaNumberDTO>().ReverseMap();
			CreateMap<VillaNumberCreateDTO,VillaNumber>().ReverseMap();
			CreateMap<VillaNumberUpdateDTO, VillaNumber>().ReverseMap();

		}

    }
}
