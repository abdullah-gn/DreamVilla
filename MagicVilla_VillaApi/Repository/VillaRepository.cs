using DreamVilla_VillaApi.Data;
using DreamVilla_VillaApi.Models;
using DreamVilla_VillaApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DreamVilla_VillaApi.Repository
{
	public class VillaRepository : Repository<Villa> , IVillaRepository 
	{
		private readonly ApplicationDbContext _db;
		public VillaRepository(ApplicationDbContext dbContext):base(dbContext)
        {
			_db = dbContext;
        }
		
		public async Task<Villa> updateAsync(Villa VillaEntity)
		{
			VillaEntity.UpdatedDate = DateTime.Now;
			_db.Villas.Update(VillaEntity);
			await _db.SaveChangesAsync();
			return VillaEntity;
		}

		
	}
}
