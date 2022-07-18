using CashRegister.Application.Interfaces;
using CashRegister.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashRegister.Infrastructure;
using CashRegister.Domain.Models;
using CashRegister.Infrastructure.Intrface;
using CashRegister.Application.Request;
using CashRegister.Domain.DomainModels;
using CashRegister.Domain.Exceptions;
using CashRegister.Infrastructure.Interfaces;

namespace CashRegister.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task AddProduct(CreateProductRequest product)
        {
            Product newProduct = new Product
            {
                Name = product.ProductName,
                Price = product.ProductPrice,
            };
            _productRepository.Insert(newProduct);
            _productRepository.Save();
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<ProductToShowDomenModel>> GetAllAsync()
        {
            var data = await _productRepository.GetAllAsync();

            List<ProductToShowDomenModel> result = new List<ProductToShowDomenModel>();
            ProductToShowDomenModel ProductDTO;

            foreach (var item in data)
            {
                ProductDTO = new ProductToShowDomenModel
                {
                    ProductID = item.Id,
                    ProductName = item.Name,
                    ProductPrice = item.Price,
                };

                result.Add(ProductDTO);
            }

            return result;
        }

        public async Task<ProductToShowDomenModel> GetByPKAsync(object PK)
        {
            var products = await _productRepository.GetByPKAsync(PK);
            if (products is null)
                throw new ProductNotFoundException("Wrong product ID!");

            ProductToShowDomenModel ProductDTO;

            ProductDTO = new ProductToShowDomenModel
            {
                ProductID = products.Id,
                ProductName = products.Name,
                ProductPrice = products.Price
            };

            return ProductDTO;
        }
        public async Task Delete(object PK)
        {
            var existingProduct = await _productRepository.GetByPKAsync(PK);
            if (existingProduct is null)
                throw new ProductNotFoundException("Wrong product ID!");

            await _productRepository.Delete(PK);
            _productRepository.Save();
        }

    }
}
