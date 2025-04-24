using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data_Access.Interfaces
{
    public interface IRequirementRepository<T> : IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(); 
    }
}
