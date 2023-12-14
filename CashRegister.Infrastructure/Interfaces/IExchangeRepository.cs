using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Infrastructure.Interfaces
{
    public interface IExchangeRepository
    {
        double Exchange(string currency);
    }
}
