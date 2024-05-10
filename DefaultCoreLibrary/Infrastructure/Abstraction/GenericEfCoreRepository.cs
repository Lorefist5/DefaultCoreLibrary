using DefaultCoreLibrary.Domain.Abstraction;
using DefaultCoreLibrary.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DefaultCoreLibrary.Infrastructure.Abstraction;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : Entity
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _entities;
    public GenericRepository(DbContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }
    public virtual void Add(T entity)
    {
        _entities.Add(entity);
    }

    public virtual async Task AddAsync(T entity)
    {
        await _entities.AddAsync(entity);
    }

    public virtual T? FirstOrDefault(Expression<Func<T, bool>> predicate)
    {
        return _entities.FirstOrDefault(predicate);
    }

    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _entities.FirstOrDefaultAsync(predicate);
    }

    public virtual IEnumerable<T> GetAll()
    {
        return _entities.ToList();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _entities.ToListAsync();
    }

    public virtual void Remove(T entity)
    {
        var newEntity = _entities.FirstOrDefault(e => e.Id == entity.Id);
        if (newEntity != null)
        {
            _entities.Remove(newEntity);
        }
    }

    public virtual async Task RemoveAsync(T entity)
    {
        var newEntity = await _entities.FirstOrDefaultAsync(e => e.Id == entity.Id);
        if (newEntity != null)
        {
            _entities.Remove(newEntity);
        }
    }

    public virtual IEnumerable<T> Where(Expression<Func<T, bool>> predicate) => _entities.Where(predicate);


    public virtual Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate) => _entities.Where(predicate).ToListAsync();
    public virtual async Task<T?> UpdateAsync(T newValues)
    {
        T? existingEntity = await _entities.FirstOrDefaultAsync(e => e.Id == newValues.Id);
        if (existingEntity == null) return null;
        _entities.Update(newValues);
        return existingEntity;
    }
    public virtual T? Update(T newValues)
    {
        T? existingEntity = _entities.FirstOrDefault(e => newValues.Id == e.Id);
        if (existingEntity == null) return null;
        _entities.Update(newValues);
        return existingEntity;
    }
}