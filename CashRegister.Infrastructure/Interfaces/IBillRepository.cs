using CashRegister.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Infrastructure.Intrface
{
    public interface IBillRepository : IRepository<Bill>
    { 
    }
}
