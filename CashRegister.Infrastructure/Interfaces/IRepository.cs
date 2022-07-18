using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Infrastructure.Intrface
{
    public interface IRepository<T> where T : class
    {
        Task<T> Delete(object PK);
        Task<T> GetByPKAsync(object PK);
        Task<IEnumerable<T>> GetAllAsync();
        T Insert(T obj);
        T Update(T obj);
        void Save();

    }
}
