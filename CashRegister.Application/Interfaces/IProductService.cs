using CashRegister.Application.Request;
using CashRegister.Domain.DomainModels;
using CashRegister.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductToShowDomenModel>> GetAllAsync();
        Task<ProductToShowDomenModel> GetByPKAsync(object PK);
        Task AddProduct(CreateProductRequest product);
        Task Delete(object PK);
    }
}
