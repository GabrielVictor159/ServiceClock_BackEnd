
using System.Linq.Expressions;

namespace ServiceClock_BackEnd.Application.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    T? GetById(dynamic id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Delete(T entity);
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize);
    IEnumerable<T> FindContainIncludes(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, params Expression<Func<T, object>>[] includes);
    T? FindSingle(Expression<Func<T, bool>> predicate);
    int Count(Expression<Func<T, bool>> predicate);
    void AddRange(IEnumerable<T> entities);
    void UpdateRange(IEnumerable<T> entities);
    void DeleteRange(IEnumerable<T> entities);
    int Update(T entity);
    int Save();
}

