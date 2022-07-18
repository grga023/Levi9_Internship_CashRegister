using CashRegister.Domain.Models;
using CashRegister.Infrastructure.Intrface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Infrastructure.Interfaces
{
    public interface IProductBillRepository<T> where T : class
    {
        Task<ProductBill> GetAllByPKAsync(object PK_B, object PK_P);
        int GetTotalPriceByBillNumber(object PK_B);
        Task<T> Delete(object PK_B, object PK_P);
        T Insert(T obj);
        T Update(T obj);
        void Save();
    }
}
