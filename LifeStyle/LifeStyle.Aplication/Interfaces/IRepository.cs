

namespace LifeStyle.LifeStyle.Aplication.Interfaces
{
    public interface IRepository<T>
    {
        Task Add(T entity);
        Task Remove(T entity);
        Task Update(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetById(int id);

    }

}
