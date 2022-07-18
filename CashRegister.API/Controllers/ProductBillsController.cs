using CashRegister.Application.Interfaces;
using CashRegister.Application.Request;
using CashRegister.Domain.DomainModels;
using CashRegister.Domain.Exceptions;
using CashRegister.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CashRegister.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBillsController : ControllerBase
    {

        private readonly IProductBillService _productBillService;

        public ProductBillsController(IProductBillService productBillService)
        {
            _productBillService = productBillService;
        }

        [HttpDelete]
        [Route("{BillNumber}/product/{ProductId}")]
        public async Task<ActionResult> DeleteProductFromBill([FromRoute(Name = "BillNumber")]string PK_B,[FromRoute(Name = "ProductId")] int PK_P)
        {
            try
            {
                await _productBillService.DeleteProductFromBill(PK_B, PK_P);
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

        [HttpPut]
        [Route("{BillNumber}/product/{ProductId}")]
        public async Task<ActionResult> UpdateProductOnBill([FromBody]UpdateProductOnBillRequest updateProductsOnBill, [FromRoute(Name = "BillNumber")] string PK_B, [FromRoute(Name = "ProductId")] int PK_P)
        {
            try 
            {
                await _productBillService.UpdateProductOnBill(updateProductsOnBill, PK_B, PK_P);
                return Ok();
            } 
            catch(Exception ex)
            {
                ErrorModel error = new()
                {
                    Error = ex.Message,
                };
                return BadRequest(error);
            }
        }

        [HttpPut]
        [Route("{BillNumber}")]
        public async Task<ActionResult> InsertProductToBill([FromBody]CreateBillProductRequestModel obj, [FromRoute(Name = "BillNumber")] string PK_B)
        {
            try
            {
                await _productBillService.InsertProductOnBill(obj, PK_B);
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
