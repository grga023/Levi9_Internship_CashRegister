using CashRegister.Domain.Models;
using CashRegister.Infrastructure.Context;
using CashRegister.Infrastructure.Intrface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public CashRegisterDBContext _CashRegContext;

        public ProductRepository(CashRegisterDBContext cashRegContext)
        {
            _CashRegContext = cashRegContext;
        }
    
        public async Task<Product> Delete(object PK)
        {
            var existing = await GetByPKAsync(PK);
            var result = _CashRegContext.Products.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
           var data = await _CashRegContext.Products
                .ToListAsync();

           return data;
        }

        public async Task<Product> GetByPKAsync(object PK)
        { 
            var data = await _CashRegContext.Products
                .Include(x => x.ProductBills)
                .ThenInclude(x => x.Bill)
                .FirstOrDefaultAsync(x => x.Id == (int)PK);

            return data;
        }

        public Product Insert(Product obj)
        {
            return _CashRegContext.Products.Add(obj).Entity;
        }

        public void Save()
        {
            _CashRegContext.SaveChanges();
        }

        public Product Update(Product obj)
        {
            _CashRegContext.Entry(obj).State = EntityState.Modified;

            return obj;
        }
    }
}
