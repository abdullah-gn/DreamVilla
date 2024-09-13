using DreamVilla_VillaApi.Data;
using DreamVilla_VillaApi.Models;
using DreamVilla_VillaApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

		public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool IsTracked = true, string? IncludeProperties = null)
		{
			IQueryable<T> query = DbSet;

			if (!IsTracked)
			{
				query = query.Where(filter).AsNoTracking();
			}
			if (filter != null)
			{
				query = query.Where(filter);
			}
			//Check for Navigation props

			if (IncludeProperties != null)
			{
				foreach (var property in IncludeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(property);
				}
			}
			return await query.FirstOrDefaultAsync();
		}

		public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? IncludeProperties = null,
            int PageSize = 0, int PageNumber = 1)
		{
			IQueryable<T> query = DbSet;

			if (filter != null)
			{

				query = query.Where(filter);
			}

			if (PageSize > 0)
			{
				if (PageNumber > 100)
				{
                    PageNumber = 100;
				}
				query = query.Skip(PageSize * (PageNumber-1)).Take(PageSize);
			}



			if (IncludeProperties != null)
			{
				foreach (var property in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(property);
				}
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
