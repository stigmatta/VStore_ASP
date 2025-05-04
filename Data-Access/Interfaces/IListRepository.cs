namespace Data_Access.Interfaces
{
    public interface IListRepository<T> : IRepository<T> where T : class
    {
        Task<IEnumerable<T?>> GetAll(); 
    }
}
