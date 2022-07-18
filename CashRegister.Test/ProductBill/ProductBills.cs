using CashRegister.Application.Request;
using CashRegister.Application.Services;
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
using Xunit;

namespace CashRegister.Test.ProductBill
{
    [TestFixture]
    public class ProductBills
    {
        private Mock<IBillRepository> _billRepositoryMock;
        private Mock<IProductRepository> _productRepositorymock;
        private Mock<IProductBillRepository<Domain.Models.ProductBill>> _productBillRepositoryMock;
        private ProductBillService _productBillService;

        [SetUp]
        public void SetUp()
        {
            _billRepositoryMock = new Mock<IBillRepository>();
            _productRepositorymock = new Mock<IProductRepository>();
            _productBillRepositoryMock = new Mock<IProductBillRepository<Domain.Models.ProductBill>>();
            _productBillService = new ProductBillService(_productBillRepositoryMock.Object, _billRepositoryMock.Object, _productRepositorymock.Object);
        }

        [Test]
        public async Task DeleteProductFromBill_WhenCalled_DeleteProduct()
        {
            var repo = _productBillRepositoryMock.Setup(c => c.GetAllByPKAsync(It.IsAny<object>(),It.IsAny<object>())).ReturnsAsync(new Domain.Models.ProductBill
            {
                BillNumber = "1",
                ProductId = 1,
                ProductQuantity = 1,
                ProductsPrice = 1,
            });

            var billRepo = _billRepositoryMock.Setup(c => c.GetByPKAsync(It.IsAny<object>())).ReturnsAsync((new Domain.Models.Bill
            {
                BillNumber = "1",
                CreditCardNumber = "1",
                PaymentMethod = "card",
                TotalPrice = 1,
            }));

            await _productBillService.DeleteProductFromBill(1, 1);

            _productBillRepositoryMock.Verify(r => r.Delete(1, 1));
            _productBillRepositoryMock.Verify(r => r.Save());
            _billRepositoryMock.Verify(b => b.Update(It.IsAny<Domain.Models.Bill>()));
            _billRepositoryMock.Verify(b => b.Save());
        }

        [Test]
        public async Task UpdateProductOnBill_WhenCalled_Update()
        {
            var repo = _productBillRepositoryMock.Setup(c => c.GetAllByPKAsync(It.IsAny<object>(), It.IsAny<object>())).ReturnsAsync(new Domain.Models.ProductBill
            {
                BillNumber = "1",
                ProductId = 1,
                ProductQuantity = 1,
                ProductsPrice = 1,
            });

            var billRepo = _billRepositoryMock.Setup(c => c.GetByPKAsync(It.IsAny<object>())).ReturnsAsync((new Domain.Models.Bill
            {
                BillNumber = "1",
                CreditCardNumber = "1",
                PaymentMethod = "card",
                TotalPrice = 1,
            }));

            var productRepo = _productRepositorymock.Setup(c => c.GetByPKAsync(It.IsAny<object>())).ReturnsAsync((new Product
            {
                Id = 1,
                Name = "A",
                Price = 1
            }));

            UpdateProductOnBillRequest obj = new()
            {
                ProductQuantity = 1,
            };

            await _productBillService.UpdateProductOnBill(obj, 1, 1);

            _productBillRepositoryMock.Verify(r => r.Update(It.IsAny<Domain.Models.ProductBill>()));
            _productBillRepositoryMock.Verify(r => r.Save());
            _billRepositoryMock.Verify(b => b.Update(It.IsAny<Domain.Models.Bill>()));
            _billRepositoryMock.Verify(b => b.Save());
        }

        [Test]
        public async Task UpdateProductOnBill_WrongCall_ThrowException()
        {
            UpdateProductOnBillRequest obj = new()
            {
                ProductQuantity = 1,
            };

            var exceotion = await Record.ExceptionAsync(async () => await _productBillService.UpdateProductOnBill(obj, 1, 1));

            Assert.That(() => exceotion.Message, Is.EqualTo("Wrong product id or bill number!"));
        }

        [Test]
        public async Task InsertOnBill_WhenCalled_InsertPtoductInDB()
        {
            var repo = _productBillRepositoryMock.Setup(c => c.GetAllByPKAsync(It.IsAny<object>(), It.IsAny<object>())).ReturnsAsync(new Domain.Models.ProductBill
            {
                BillNumber = "1",
                ProductId = 1,
                ProductQuantity = 1,
                ProductsPrice = 1,
            });

            var billRepo = _billRepositoryMock.Setup(c => c.GetByPKAsync(It.IsAny<object>())).ReturnsAsync((new Domain.Models.Bill
            {
                BillNumber = "1",
                CreditCardNumber = "1",
                PaymentMethod = "card",
                TotalPrice = 1,
            }));

            var productRepo = _productRepositorymock.Setup(c => c.GetByPKAsync(It.IsAny<object>())).ReturnsAsync(new Product
            {
                Id = 1,
                Name = "A",
                Price = 1
            });

            CreateBillProductRequestModel obj = new CreateBillProductRequestModel(1, 1);

            var exception = await Record.ExceptionAsync(async () => await _productBillService.InsertProductOnBill(obj, 1));

            Assert.That(() => exception.Message, Is.EqualTo("Product already exists on that bill!"));
        }
    }
}
