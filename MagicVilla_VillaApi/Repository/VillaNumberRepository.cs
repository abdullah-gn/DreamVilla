using DreamVilla_VillaApi.Data;
using DreamVilla_VillaApi.Models;
using DreamVilla_VillaApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DreamVilla_VillaApi.Repository
{
	public class VillaNumberRepository:Repository<VillaNumber> , IVillaNumberRepository
	{
        private readonly DbContext _dbContext;
        public VillaNumberRepository(ApplicationDbContext dbCotext):base(dbCotext)
        {
                _dbContext = dbCotext;
        }

		public async Task<VillaNumber> updateAsync(VillaNumber VillaNumberEntity)
		{
			VillaNumberEntity.UpdatedDate = DateTime.Now;
			_dbContext.Update(VillaNumberEntity);
			await SaveAsync();
			return VillaNumberEntity;
		}
	}
}
