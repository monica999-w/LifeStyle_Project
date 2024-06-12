using LifeStyle.Application.Exercises.Responses;
using LifeStyle.Domain.Models.Exercises;
using System.Linq.Expressions;

namespace LifeStyle.Aplication.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> Add(T entity);
        Task<T> Remove(T entity);
        Task<T> Update(T entity);
        Task<List<T>> GetAll();
        Task<T?> GetById(int id);
        int GetLastId();
        Task<T> GetByName(string name);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    }

}
