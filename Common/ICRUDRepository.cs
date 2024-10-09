using System.Data;

namespace Common
{
    public interface ICRUDRepository<TEntity, TId>
    {
        public IEnumerable<TEntity> GetAll();
        public TEntity GetById(TId id);
        public int Add(TEntity entity);
        public bool Update(TId id, TEntity entity);
        public bool Delete(int id);
    }
}
