using CashRegister.Application.Request;
using CashRegister.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Application.Interfaces
{
    public interface IProductBillService
    {
        Task InsertProductOnBill(CreateBillProductRequestModel obj, object PK_B);
        Task DeleteProductFromBill(object PK_B, object PK_P);
        Task UpdateProductOnBill(UpdateProductOnBillRequest obj, object PK, object PK_P);
        int GetTotalPriceByBillNumber(object PK_B);
    }
}
