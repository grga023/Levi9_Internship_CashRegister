using CashRegister.Domain.DomainModels;
using CashRegister.Domain.Exceptions;
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
    public class BillRepository : IBillRepository
    {
        public CashRegisterDBContext _ctx;

        public BillRepository(CashRegisterDBContext cashRegContext)
        {
            _ctx = cashRegContext;
        }

        public async Task<Bill> Delete(object PK)
        {
            Bill existing = await GetByPKAsync(PK);
            if(existing is null)
                throw new BillNotFoundException("Wrong bill number!");

            var result = _ctx.Bills.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<Bill>> GetAllAsync()
        {
            var data = await _ctx.Bills
                .Include(x => x.ProductBills )
                .ThenInclude(x => x.Product)
                .ToListAsync();

            return data; 
        }

        public async Task<Bill> GetByPKAsync(object PK)
        {
            var data = await _ctx.Bills
                .Include(x => x.ProductBills)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.BillNumber == PK);



            return data;
        }

        public Bill Insert(Bill obj)
        {
            return _ctx.Bills.Add(obj).Entity;
        }

        public void Save()
        {
            _ctx.SaveChanges();
        }

        public  Bill Update(Bill obj)
        {
            try
            {
                  _ctx.Entry(obj).State = EntityState.Modified;

                return obj;
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }
    }
}
