using DefaultCoreLibrary.Domain.Abstraction;
using System.Linq.Expressions;
namespace DefaultCoreLibrary.Domain.Interfaces;

public interface IGenericRepository<T> where T : Entity
{
    public T? FirstOrDefault(Expression<Func<T, bool>> predicate);
    public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    public IEnumerable<T> Where(Expression<Func<T, bool>> predicate);
    public Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate);
    public void Add(T entity);
    public Task AddAsync(T entity);
    public void Remove(T entity);
    public Task RemoveAsync(T entity);
    public Task<IEnumerable<T>> GetAllAsync();
    public IEnumerable<T> GetAll();
    public T? Update(T newValues);
    public Task<T?> UpdateAsync(T newValues);
}
