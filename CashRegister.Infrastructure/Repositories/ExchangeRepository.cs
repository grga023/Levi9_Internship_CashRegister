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
    public class ExchangeRepository : IExchangeRepository<Bill>
    {
        public double Exchange(string currency)
        {
            double cours;

            switch (currency)
            {
                case "EUR":
                    cours = 0.0085;
                    break;
                case "USD":
                    cours = 0.0090;
                    break;
                default:
                    cours = 1;
                    break;
            }

            return cours;
        }
    }
}
