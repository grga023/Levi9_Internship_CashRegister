using CashRegister.Application.Interfaces;
using CashRegister.Domain.DomainModels;
using CashRegister.Domain.Models;
using CashRegister.Infrastructure.Interfaces;
using CashRegister.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Application.Services
{
    public class ExchangeService : IExchangeService
    {
        private readonly IExchangeRepository _exchangeRepository;

        public ExchangeService(IExchangeRepository exchangeRepository)
        {
            _exchangeRepository = exchangeRepository;
        }

        public BillToShowDomenModel ExchangeBillCurrency(BillToShowDomenModel obj, string currency)
        {
            double totalPrice = obj.totalPrice;

            foreach(var item in obj.ProductList)
            {
                item.ProductPrice = ExchangeCurrency(item.ProductPrice, currency);
                item.ProductsPrice = ExchangeCurrency(item.ProductsPrice, currency);
            }

            totalPrice = ExchangeCurrency(totalPrice, currency);

            obj.totalPrice = totalPrice;

            return obj;
        }

        private double Truncate(double a)
        {
            return Math.Truncate(a * 100) / 100; ;
        }

        private double ExchangeCurrency(double totalPrice, string currency)
        {
            switch (currency)
            {
                case "RSD":
                    break;

                case "EUR":
                    totalPrice *= _exchangeRepository.Exchange(currency);
                    break;

                case "USD":
                    totalPrice *= _exchangeRepository.Exchange(currency);
                    break;

                default:
                    throw new ArgumentException("Wrong currency!");
            }

            totalPrice = Truncate(totalPrice);

            return totalPrice;
        }

    }
}
