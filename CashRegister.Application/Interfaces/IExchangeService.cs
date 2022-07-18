using CashRegister.Domain.DomainModels;
using CashRegister.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Application.Interfaces
{
    public interface IExchangeService
    {
        BillToShowDomenModel ExchangeBillCurrency(BillToShowDomenModel obj, string currency);
    }
}
