using CashRegister.Domain.Models;
using CashRegister.Infrastructure.Context;
using CashRegister.Infrastructure.Interfaces;
using CashRegister.Infrastructure.Intrface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Infrastructure.Repositories
{
    public class ProductBillRepository : IProductBillRepository<ProductBill>
    {

        public readonly CashRegisterDBContext _ctx;

        public ProductBillRepository(CashRegisterDBContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<ProductBill> Delete(object PK_B, object PK_P)
        {
            var existing = await _ctx.ProductBills
                .FirstOrDefaultAsync(x => x.ProductId == (int)PK_P && x.BillNumber == PK_B);
                
            var result = _ctx.ProductBills.Remove(existing);

            return result.Entity;
        }

        public async Task<ProductBill> GetAllByPKAsync(object PK_B, object PK_P)
        {
            var data = await _ctx.ProductBills
                .FirstOrDefaultAsync(x => x.ProductId == (int)PK_P && x.BillNumber == PK_B);

            return data;
        }

        public int GetTotalPriceByBillNumber(object PK_B)
        {
            var data = _ctx.ProductBills.Where(x => x.BillNumber == PK_B).Sum(i => i.ProductsPrice);

            return data;
        }



        public ProductBill Insert(ProductBill obj)
        {
            return _ctx.ProductBills.Add(obj).Entity;
        }

        public void Save()
        {
            _ctx.SaveChanges();
        }

        public ProductBill Update(ProductBill obj)
        {
            _ctx.Entry(obj).State = EntityState.Modified;

            return obj;
        }
    }
}
