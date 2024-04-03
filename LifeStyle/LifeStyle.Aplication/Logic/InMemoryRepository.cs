using LifeStyle.LifeStyle.Aplication.Interfaces;

namespace LifeStyle.LifeStyle.Aplication.Logic
{

    public class InMemoryRepository<T> : IRepository<T>
    {
        private readonly List<T> _entities = new List<T>();

        public async Task<IEnumerable<T>> GetAll()
        {
            await Task.Delay(0);
            return _entities.ToList();
        }

        public async Task Add(T entity)
        {
            await Task.Delay(0);
            _entities.Add(entity);
        }

        public async Task Remove(T entity)
        {
            await Task.Delay(0);
            _entities.Remove(entity);
        }

        public async Task Update(T entity)
        {
            await Task.Delay(0);

        }

        public async Task<T?> GetById(int id)
        {
            await Task.Delay(0);
            var entity = _entities.FirstOrDefault(e => e?.GetType().GetProperty("Id")?.GetValue(e)?.Equals(id) ?? false);
            return entity;
        }
    }
}
