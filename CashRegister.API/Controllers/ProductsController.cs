using CashRegister.Application.Interfaces;
using CashRegister.Application.Request;
using CashRegister.Domain.DomainModels;
using CashRegister.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CashRegister.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductToShowDomenModel>>> GetAllProductAsync()
        {
            try
            {
                var data = await _productService.GetAllAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                ErrorModel error = new() 
                {
                    Error = ex.Message,
                };
                return NotFound(error);
            }
        }

        [HttpGet]
        [Route("{ProductId}")]
        public async Task<ActionResult<IEnumerable<ProductToShowDomenModel>>> GetProductsByPKAsync([FromRoute(Name = "ProductId")]int PK)
        {
            try
            {
                var product = await _productService.GetByPKAsync(PK);
                return Ok(product);
            }
            catch (Exception ex)
            {
                ErrorModel error = new()
                {
                    Error = ex.Message,
                };
                return NotFound(error);
            }
        }

        [HttpPost]
        public async Task<ActionResult> InsertProduct([FromBody]CreateProductRequest obj)
        {
                await _productService.AddProduct(obj);
                return Ok();  
        }

        [HttpDelete]
        [Route("{ProductId}")]
        public async Task<ActionResult> Delete([FromRoute(Name = "productId")]int PK)
        {
            try
            {
                await _productService.Delete(PK);
                return Ok();
            }
            catch (Exception ex)
            {
                ErrorModel error = new()
                {
                    Error = ex.Message,
                };
                return BadRequest(error);
            }

        }
    }
}
