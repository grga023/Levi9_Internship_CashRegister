using CashRegister.Application.Interfaces;
using CashRegister.Application.Request;
using CashRegister.Domain.Models;
using CashRegister.Infrastructure.Interfaces;
using CashRegister.Infrastructure.Intrface;
using CashRegister.Infrastructure.Repositories;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CashRegister.Domain.Exceptions;

namespace CashRegister.Application.Services
{
    public class ProductBillService : IProductBillService
    {
        private readonly IProductBillRepository<ProductBill> _productBillRepository;
        private readonly IBillRepository _billRepository;
        private readonly IProductRepository _productRepository;

        public ProductBillService(IProductBillRepository<ProductBill> productBillRepository, IBillRepository billRepository, IProductRepository productRepository)
        {
            _productBillRepository = productBillRepository;
            _billRepository = billRepository;
            _productRepository = productRepository;
        }

        public async Task DeleteProductFromBill(object PK_B, object PK_P)
        {

            var productToDelete = await _productBillRepository.GetAllByPKAsync(PK_B, PK_P);
            if (productToDelete is null)
                throw new ProductBillNotFoundException("Wrong product id or bill number!");
           
            await _productBillRepository.Delete(PK_B, PK_P);
            
            var billData = await _billRepository.GetByPKAsync(PK_B);

            _productBillRepository.Save();

            int totalPrice = GetTotalPriceByBillNumber(PK_B);
            billData.TotalPrice = totalPrice;

            _billRepository.Update(billData);
            _billRepository.Save();
        }

        public async Task UpdateProductOnBill(UpdateProductOnBillRequest obj, object PK_B, object PK_P)
        {
            var productBillData = await _productBillRepository.GetAllByPKAsync(PK_B, PK_P);
            if (productBillData is null) throw new ProductBillNotFoundException("Wrong product id or bill number!");

            var billData = await _billRepository.GetByPKAsync(PK_B);
            var productData = await _productRepository.GetByPKAsync(PK_P);

            int productsPrice = productData.Price * obj.ProductQuantity;

            ProductBill updatedProduct = new ProductBill
            {
                BillNumber = billData.BillNumber,
                ProductId = productData.Id,
                ProductQuantity = obj.ProductQuantity,
                ProductsPrice = productsPrice,
            };

            _productBillRepository.Update(updatedProduct);
            _productBillRepository.Save();

            int test = GetTotalPriceByBillNumber(PK_B);
            billData.TotalPrice = test;
           
            _billRepository.Update(billData);
            _billRepository.Save();
        }

        public async Task InsertProductOnBill(CreateBillProductRequestModel obj, object PK_B)
        {
            var billData = await _billRepository.GetByPKAsync(PK_B);
            var productData = await _productRepository.GetByPKAsync(obj.ProductId);

            var productBillData = await _productBillRepository.GetAllByPKAsync(PK_B, obj.ProductId);
            if (productBillData is not null) throw new ProductBillNotFoundException("Product already exists on that bill!");

            int quantity = obj.Quantity;    
            int productsPriceCalculated = productData.Price * quantity;

            ProductBill newProduct = new()
            {
                BillNumber = billData.BillNumber,
                ProductQuantity = quantity,
                ProductId = obj.ProductId,
                ProductsPrice = productsPriceCalculated,
            };
            
            _productBillRepository.Insert(newProduct);
            _productBillRepository.Save();

            
            int test = GetTotalPriceByBillNumber(PK_B);
            billData.TotalPrice = test;

            _billRepository.Update(billData);
            _billRepository.Save();
        }

        public int GetTotalPriceByBillNumber(object PK_B)
        {
            var total = _productBillRepository.GetTotalPriceByBillNumber(PK_B);

            return total;
        }
    }
}
