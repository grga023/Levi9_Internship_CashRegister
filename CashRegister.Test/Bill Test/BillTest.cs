using CashRegister.API.Controllers;
using CashRegister.Application.Interfaces;
using CashRegister.Application.Request;
using CashRegister.Application.Services;
using CashRegister.Domain.DomainModels;
using CashRegister.Domain.Exceptions;
using CashRegister.Domain.Models;
using CashRegister.Infrastructure.Interfaces;
using CashRegister.Infrastructure.Intrface;
using CashRegister.Infrastructure.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using Xunit;

namespace CashRegister.Test.Bill_Test
{   
    [TestFixture]
    public class BillTest
    {
        private Mock<IBillRepository> _billRepositoryMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private IValidationService _validationServiceMock;
        private BillService _billService;

        [SetUp]
        public void SetUp()
        {
            _billRepositoryMock = new Mock<IBillRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _validationServiceMock = new ValidationService();
            _billService = new BillService(_billRepositoryMock.Object, _productRepositoryMock.Object, _validationServiceMock);
        }


        [Test]
        public async Task DeleteBill_WhenCalled_DeleteBillFromDbAsync()
        {
            await _billService.DeleteBill(1);
            
            _billRepositoryMock.Verify(r => r.Delete(1));
            _billRepositoryMock.Verify(r => r.Save());
        }

        [Test]
        public async Task GetBillByBillNumber_WhenCalled_ReturnBill()
        {
            Product products = new() { Id = 1, Name = "A", Price = 1 };

            List<Domain.Models.ProductBill> productBill = new();
            var productBillmodel = new Domain.Models.ProductBill { ProductId = 1, BillNumber = "A", ProductQuantity = 1, ProductsPrice = 1 , Product = products};
            productBill.Add(productBillmodel);

            var repo = _billRepositoryMock.Setup(c => c.GetByPKAsync(It.IsAny<object>())).ReturnsAsync((new Bill
            {
                BillNumber = "1",
                CreditCardNumber = "1",
                PaymentMethod = "card",
                TotalPrice = 1,
                ProductBills = productBill,
            }));

            var result = await _billService.GetByPKAsync(1, "RSD");

            _billRepositoryMock.Verify(r => r.GetByPKAsync(1));
            Assert.That(result.totalPrice, Is.EqualTo(1));
            Assert.That(result.BillNumber, Is.EqualTo("1"));
            Assert.That(result.PaymentMethod, Is.EqualTo("card"));
            Assert.That(result.Currency, Is.EqualTo("RSD"));
            Assert.That(result.ProductList.Count, Is.EqualTo(1));
            Assert.That(result.ProductList[0].ProductName, Is.EqualTo("A"));
            Assert.That(result.ProductList[0].ProductPrice, Is.EqualTo(1));
            Assert.That(result.ProductList[0].ProductsPrice, Is.EqualTo(1));
            Assert.That(result.ProductList[0].Quantity, Is.EqualTo(1));
        }

        [Test]
        public async Task GetBillByBillNumber_WhenCalled_ThrowException()
        {
            var exception = await Record.ExceptionAsync(async () => await _billService.GetByPKAsync("1", "A"));

            Assert.That(() => exception.Message, Is.EqualTo("Wrong bill number"));
        }

        [Test]
        public async Task UpdateBill_WhenCalled_Success()
        {
            UpdateBillRequest updateObj = new()
            {
                CreditCardNumber = "5355460002643183",
                PaymentMethod = "cash",
            };

            var repo = _billRepositoryMock.Setup(c => c.GetByPKAsync(It.IsAny<object>())).ReturnsAsync((new Bill
            {
                BillNumber = "1",
                CreditCardNumber = "1",
                PaymentMethod = "card",
                TotalPrice = 1,
            }));

            await _billService.UpdateBill(updateObj, "1");

            _billRepositoryMock.Verify(c => c.Update(It.IsAny<Bill>()));
            _billRepositoryMock.Verify(c => c.Save());
        }

        [Test]
        public async Task UpdateBill_WrongCreditcardNumber_ThrowValidationException()
        {
            UpdateBillRequest updateObj = new()
            {
                CreditCardNumber = "1",
                PaymentMethod = "cash",
            };

            var repo = _billRepositoryMock.Setup(c => c.GetByPKAsync(It.IsAny<object>())).ReturnsAsync((new Bill
            {
                BillNumber = "1",
                CreditCardNumber = "1",
                PaymentMethod = "card",
                TotalPrice = 1,
            }));

            var exception = await Record.ExceptionAsync(async () => await _billService.UpdateBill(updateObj, "1"));

            Assert.That(() => exception.Message, Is.EqualTo("Invalid credit card number!"));
        }


        [Test]
        public async Task UpdateBill_WrongBillNumber_ThrowWrongBillException()
        {
            UpdateBillRequest updateObj = new()
            {
                CreditCardNumber = "1",
                PaymentMethod = "cash",
            };

            var exception = await Record.ExceptionAsync(async () => await _billService.UpdateBill(updateObj, "1"));

            Assert.That(() => exception.Message, Is.EqualTo("Wrong bill number!"));
        }

        [Test]
        public async Task CreateBill_WhenCall_CreateBill()
        {
            

            var repo = _productRepositoryMock.Setup(c => c.GetByPKAsync(It.IsAny<object>())).ReturnsAsync((new Product
            {
                Id = 1,
                Name = "A",
                Price = 1
            }));

            List<CreateBillProductRequestModel> probuctBill = new();
            CreateBillProductRequestModel productToAdd = new(1, 1);
            probuctBill.Add(productToAdd);

            CreateBillRequestModel billToAdd = new("Card", "5355460002643183", probuctBill);

            

            await _billService.CreateNewBill(billToAdd);

            _billRepositoryMock.Verify(r => r.Insert(It.IsAny<Bill>()));
            _billRepositoryMock.Verify(r => r.Save());
        }

        [Test]
        public async Task CreateBill_QuantityZero_ReturnQuantityException()
        {
            var repo = _productRepositoryMock.Setup(c => c.GetByPKAsync(It.IsAny<object>())).ReturnsAsync((new Product
            {
                Id = 1,
                Name = "A",
                Price = 1
            }));

            List<CreateBillProductRequestModel> probuctBill = new();
            CreateBillProductRequestModel productToAdd = new(0, 1);
            probuctBill.Add(productToAdd);

            CreateBillRequestModel billToAdd = new("Card", "5355460002643183", probuctBill);

            var exception = await Record.ExceptionAsync(async () => await _billService.CreateNewBill(billToAdd));

            Assert.That(() => exception.Message, Is.EqualTo("Quantity must be greater than 0!"));
        }

        [Test]
        public async Task CreateBill_WrongproductId_ReturnproductException()
        {
            List<CreateBillProductRequestModel> probuctBill = new();
            CreateBillProductRequestModel productToAdd = new(1, 1);
            probuctBill.Add(productToAdd);

            CreateBillRequestModel billToAdd = new("Card", "5355460002643183", probuctBill);

            var exception = await Record.ExceptionAsync(async () => await _billService.CreateNewBill(billToAdd));

            Assert.That(() => exception.Message, Is.EqualTo("Wrong product id!"));
        }

        [Test]
        public async Task CreateBill_WrongCreditCardon_ReturnValidationException()
        {
            List<CreateBillProductRequestModel> probuctBill = new();
            CreateBillProductRequestModel productToAdd = new(1, 1);
            probuctBill.Add(productToAdd);

            CreateBillRequestModel billToAdd = new("Card", "1", probuctBill);

            var exception = await Record.ExceptionAsync(async () => await _billService.CreateNewBill(billToAdd));

            Assert.That(() => exception.Message, Is.EqualTo("Invalid credit card number!"));
        }

    }
}
