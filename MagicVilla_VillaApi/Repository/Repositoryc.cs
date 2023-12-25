using DreamVilla_VillaApi.Data;
using DreamVilla_VillaApi.Models;
using DreamVilla_VillaApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DreamVilla_VillaApi.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _db;
		internal DbSet<T> DbSet;
		public Repository(ApplicationDbContext dbContext)
		{
			_db = dbContext;
			this.DbSet = _db.Set<T>();
		}
		public async Task AddAsync(T VillaEntity)
		{
			
			await DbSet.AddAsync(VillaEntity);
			await SaveAsync();
		}

		public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool IsTracked = true)
		{
			IQueryable<T> qurey = DbSet;

			if (!IsTracked)
			{
				qurey = qurey.Where(filter).AsNoTracking();
			}
			if (filter != null)
			{
				qurey = qurey.Where(filter);
			}
			return await qurey.FirstOrDefaultAsync();
		}

		public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
		{
			IQueryable<T> query = DbSet;

			if (filter != null)
			{

				query = query.Where(filter);
			}
			return await query.ToListAsync();

		}

		public async Task RemoveAsync(T VillaEntity)
		{
			DbSet.Remove(VillaEntity);
			await SaveAsync();
		}

		public async Task SaveAsync()
		{
			await _db.SaveChangesAsync();
		}

		

	}
}
