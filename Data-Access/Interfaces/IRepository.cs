namespace Data_Access.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task Add(T entity);
        public void Update(T entity);
        public Task Delete(Guid id);
        public Task<T?> GetById(Guid id);
    }
}
