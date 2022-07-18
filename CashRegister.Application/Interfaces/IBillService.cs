using CashRegister.Application.Request;
using CashRegister.Domain.DomainModels;
using CashRegister.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Application.Interfaces
{
    public interface IBillService
    {
        Task<IEnumerable<BillToShowDomenModel>> GetAllAsync();
        Task<BillToShowDomenModel> GetByPKAsync(object PK, string currency);
        Task CreateNewBill(CreateBillRequestModel billModel);
        Task DeleteBill(object PK);
        Task UpdateBill(UpdateBillRequest obj, object PK);
    }
}
