namespace Data_Access.Interfaces
{
    public interface IManyToMany<T, U> where T: struct where U : struct
    {
        public Task Add(T entity, U entity2);
        public Task Delete(T entity, U entity2);
    }
}
