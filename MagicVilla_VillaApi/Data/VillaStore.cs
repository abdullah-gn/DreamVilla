using DreamVilla_VillaApi.Models.Dto;

namespace DreamVilla_VillaApi.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> VillaList = new List<VillaDTO>
        {
            new VillaDTO{id=1,name="Safari",sqft=300,occupancy=2},
            new VillaDTO{id=2,name="Bussiness",sqft=400,occupancy=4},
            new VillaDTO{id=3,name="Private",sqft=150,occupancy=1}
        };
    }
}
