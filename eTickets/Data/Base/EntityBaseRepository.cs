using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace eTickets.Data.Base;

public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
{
    private readonly AppDbContext _appDbContext;

    public EntityBaseRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(T entity)
    {
        await _appDbContext.Set<T>().AddAsync(entity);
        await _appDbContext.SaveChangesAsync();
    }


    public async Task DeleteAsync(int id)
    {
        var result = await _appDbContext.Set<T>().FirstOrDefaultAsync(a => a.Id == id);
        EntityEntry entityEntry = _appDbContext.Entry<T>(result);
        entityEntry.State = EntityState.Deleted;
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()=> await _appDbContext.Set<T>().ToListAsync();

    public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _appDbContext.Set<T>();
        query = includeProperties.Aggregate(query, (current, includeProperties) => current.Include(includeProperties));
        return await query.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)=> await _appDbContext.Set<T>().FirstOrDefaultAsync(a => a.Id == id);


    public async Task UpdateAsync(int id, T entity)
    {
        EntityEntry entityEntry =_appDbContext.Entry<T>(entity);
        entityEntry.State = EntityState.Modified;     
        await _appDbContext.SaveChangesAsync();
    }
}
