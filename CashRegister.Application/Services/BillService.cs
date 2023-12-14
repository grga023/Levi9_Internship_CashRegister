using CashRegister.Application.Interfaces;
using CashRegister.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CashRegister.Domain.DomainModels;
using CashRegister.Infrastructure.Repositories;
using CashRegister.Infrastructure.Intrface;

using CashRegister.Application.Request;
using System.Threading;
using CashRegister.Domain.Exceptions;

namespace CashRegister.Application.Services
{
    public class BillService : IBillService
    {

        private readonly IBillRepository _billRepository;
        private readonly IProductRepository _productRepository;
        private readonly IValidationService _validationService;

        public BillService(IBillRepository billRepository, IProductRepository productRepository, IValidationService validationService)
        {
            _billRepository = billRepository;
            _productRepository = productRepository;
            _validationService = validationService;
        }

        private string GenerateRandomBillNumber()
        {
            string billNumberToControll;
            string billNumber;

            Random generator = new Random();
            int dig = generator.Next(1, 1000);
            //lending zero
            string First = dig.ToString("000");
            int dig2 = generator.Next(1, 1000000000);
            int dig3 = generator.Next(1, 10000);
            string Second = dig2.ToString("000000000") + dig3.ToString("0000");

            billNumberToControll = First + Second;
            long numberBody = long.Parse(billNumberToControll);

            long controlNumber = 98 - ((numberBody * 100) % 97);
            string controlString = controlNumber.ToString("00");

            billNumber = First +"-"+ Second +"-"+ controlString;

            if (!_validationService.IsValidBillNumber(billNumber))
            {
                GenerateRandomBillNumber();
            }

            return billNumber;
        }

        private string IsValid(string cardNumber)
        { 

            if (!_validationService.isValidCreditCard(cardNumber))
                throw new ValidationFailedException("Invalid credit card number!");

            return null;
        }


        public async Task CreateNewBill(CreateBillRequestModel newBill)
        {
            //Add new bill number
            string billNumber = GenerateRandomBillNumber();
            
            //Bill exist
            var existingBill = await _billRepository.GetByPKAsync(billNumber);
            if (existingBill is not null)
                await CreateNewBill(newBill);

            //is Valid card 
            IsValid(newBill.CardNumber);

            int totalPrice = 0;
            
            Bill billToAdd = new Bill
            {
                BillNumber = billNumber,
                CreditCardNumber = newBill.CardNumber,
                PaymentMethod = newBill.PaymentMethod,
            };

            List<ProductBill> productBillList = new List<ProductBill>();

            foreach (var billProducts  in newBill.Products)
            {
                Product product = await _productRepository.GetByPKAsync(billProducts.ProductId);
                if (product is null)
                    throw new ProductNotFoundException("Wrong product id!");

                if (billProducts.Quantity == 0)
                    throw new ArgumentException("Quantity must be greater than 0!");

                int productsPriceCalculated = product.Price * billProducts.Quantity;

                productBillList.Add(new ProductBill
                {
                    BillNumber = billNumber,
                    ProductId = billProducts.ProductId,
                    ProductQuantity = billProducts.Quantity,
                    ProductsPrice = productsPriceCalculated
                });

                totalPrice += productsPriceCalculated;
            }
            billToAdd.TotalPrice = totalPrice;
            billToAdd.ProductBills = productBillList;
            _billRepository.Insert(billToAdd);
            _billRepository.Save();
        }




        public async Task DeleteBill(object PK)
        {
            await _billRepository.Delete(PK);

            _billRepository.Save();
        }



        public async Task<IEnumerable<BillToShowDomenModel>> GetAllAsync()
        {
            var billData = await _billRepository.GetAllAsync();
            if (billData is null) throw new BillNotFoundException("Please insert bill first!");

            List<BillToShowDomenModel> result = new List<BillToShowDomenModel>();
            BillToShowDomenModel billToShow;
            ProductBillToShowDomainModel productDTO;


            foreach (var item in billData)
            {
                billToShow = new BillToShowDomenModel
                {
                    BillNumber = item.BillNumber,
                    PaymentMethod = item.PaymentMethod,
                    totalPrice = item.TotalPrice,
                    Currency = "RSD",
                };

                List<ProductBillToShowDomainModel> productBillsList = new();
                
                foreach (var item2 in item.ProductBills)
                {
                    productDTO = new ProductBillToShowDomainModel
                    {
                        Quantity = item2.ProductQuantity,
                        ProductsPrice = item2.ProductsPrice,
                        ProductName = item2.Product.Name,
                        ProductPrice = item2.Product.Price,
                    };
                    productBillsList.Add(productDTO);
                };
                billToShow.ProductList = productBillsList;
                result.Add(billToShow);
            }

            return result;
        }



        public async Task<BillToShowDomenModel> GetByPKAsync(object PK, string currency)
        {
            var data = await _billRepository.GetByPKAsync(PK);
            if (data is null)
                throw new BillNotFoundException("Wrong bill number");

            List<BillToShowDomenModel> result = new List<BillToShowDomenModel>();
            BillToShowDomenModel billToShow;
            ProductBillToShowDomainModel productDTO;



            billToShow = new BillToShowDomenModel
            {
                BillNumber = data.BillNumber,
                PaymentMethod = data.PaymentMethod,
                totalPrice = data.TotalPrice,
                Currency = currency,
            };

            List<ProductBillToShowDomainModel> productBillsList = new();
            ;
            foreach (var item in data.ProductBills)
            {
                productDTO = new ProductBillToShowDomainModel
                {
                    Quantity = item.ProductQuantity,
                    ProductsPrice = item.ProductsPrice,
                    ProductName = item.Product.Name,
                    ProductPrice = item.Product.Price,
                };
                productBillsList.Add(productDTO);
            };
            billToShow.ProductList = productBillsList;
            result.Add(billToShow);

            return billToShow;
        }

        public async Task UpdateBill(UpdateBillRequest obj, object PK)
        {
            var data = await _billRepository.GetByPKAsync(PK);
            if (data == null) throw new BillNotFoundException("Wrong bill number!");

            IsValid(obj.CreditCardNumber);

            Bill BillToUpdate = new Bill
            {
                BillNumber = data.BillNumber,
                PaymentMethod = obj.PaymentMethod,
                CreditCardNumber = obj.CreditCardNumber,
                TotalPrice = data.TotalPrice,
            };

            _billRepository.Update(BillToUpdate);
            _billRepository.Save();
        }
    }
}

