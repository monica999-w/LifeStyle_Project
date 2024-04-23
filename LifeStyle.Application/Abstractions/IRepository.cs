using LifeStyle.Domain.Models.Exercises;

namespace LifeStyle.Aplication.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> Add(T entity);
        Task<T> Remove(T entity);
        Task<T> Update(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetById(int id);
        int GetLastId();
        Task<T> GetByName(string name);
    }

}
