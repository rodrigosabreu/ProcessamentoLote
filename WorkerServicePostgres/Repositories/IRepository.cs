namespace WorkerServicePostgres.Repositories;
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task DeleteRangeAsync(IEnumerable<Guid> ids);
}