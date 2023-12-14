using CashRegister.Application.Interfaces;
using CashRegister.Application.Services;
using CashRegister.Domain.DomainModels;
using CashRegister.Domain.Models;
using CashRegister.Infrastructure.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CashRegister.Tests.Application.Services
{
    [TestFixture]
    public class ExchangeServiceTests
    {
        [Test]
        public void ExchangeBillCurrency_WhenValidCurrency_ReturnsModifiedBill()
        {
            var exchangeRepositoryMock = new Mock<IExchangeRepository>();
            exchangeRepositoryMock.Setup(e => e.Exchange("USD")).Returns(1.2);

            var exchangeService = new ExchangeService(exchangeRepositoryMock.Object);

            var bill = new BillToShowDomenModel
            {
                BillNumber = "XXXXX-XXXX-XX",
                Currency = "RSD",
                PaymentMethod = "Card",
                totalPrice = 100,
                ProductList = new List<ProductBillToShowDomainModel> { new ProductBillToShowDomainModel { Quantity = 50, ProductName = "Koca", ProductPrice = 2 , ProductsPrice = 100} }
            };

            var result = exchangeService.ExchangeBillCurrency(bill, "USD");

            Assert.That(result.totalPrice, Is.EqualTo(120));
            Assert.That(result.ProductList[0].ProductPrice, Is.EqualTo(2.4));
            Assert.That(result.ProductList[0].ProductsPrice, Is.EqualTo(120));
        }

        [Test]
        public void ExchangeBillCurrency_WhenInvalidCurrency_ThrowsException()
        {
            var exchangeRepositoryMock = new Mock<IExchangeRepository>();
            var exchangeService = new ExchangeService(exchangeRepositoryMock.Object);

            var bill = new BillToShowDomenModel
            {
                BillNumber = "XXXXX-XXXX-XX",
                Currency = "RSD",
                PaymentMethod = "Card",
                totalPrice = 100,
                ProductList = new List<ProductBillToShowDomainModel> { new ProductBillToShowDomainModel { Quantity = 50, ProductName = "Koca", ProductPrice = 2, ProductsPrice = 100 } }
            };

            Assert.Throws<ArgumentException>(() => exchangeService.ExchangeBillCurrency(bill, "InvalidCurrency"));
        }
    }
}
