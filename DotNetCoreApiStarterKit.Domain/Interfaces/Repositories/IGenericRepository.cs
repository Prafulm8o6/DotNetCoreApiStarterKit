using System.Linq.Expressions;

namespace DotNetCoreApiStarterKit.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        T Add(T entity);
        T Update(T entity);
        T Remove(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);
        void UpdateRange(IEnumerable<T> entities);
        void RemoveRange(IEnumerable<T> entities);

        Task<IList<T>> GetAllAsync();

        Task<IList<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

        Task<IList<T>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

        Task<IList<T>> GetAllAsNoTrackingAsync();
        Task<T?> FirstOrDefaultAsNoTrackingAsync(Expression<Func<T, bool>> predicate);
    }
}
