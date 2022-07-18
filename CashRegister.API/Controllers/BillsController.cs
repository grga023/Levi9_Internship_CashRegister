using CashRegister.Application.Interfaces;
using CashRegister.Application.Request;
using CashRegister.Domain.Exceptions;
using CashRegister.Domain.DomainModels;
using CashRegister.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CashRegister.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly IBillService _BillService;
        private readonly IExchangeService _exchangeServic;

        public BillsController(IBillService billService, IExchangeService exchangeService)
        {
            _BillService = billService;
            _exchangeServic = exchangeService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<BillToShowDomenModel>>> GetAllBillsAsync()
        {
            try
            {
                var bills = await _BillService.GetAllAsync();
                return Ok(bills);
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
        [Route("{BillNumber}")]
        public async Task<ActionResult<BillToShowDomenModel>> GetByPKBCurrencyillsAsync([FromRoute(Name = "BillNumber")] string PK)
        {
            try
            {
                var bill = await _BillService.GetByPKAsync(PK, "RSD");
                return Ok(bill);
            }
            catch (Exception ex)
            {
                ErrorModel error = new()
                {
                    Error = ex.Message
                };
                return BadRequest(error);
            }
        }

        [HttpGet]
        [Route("{BillNumber}/currency/{currency}")]
        public async Task<ActionResult<BillToShowDomenModel>> GetByPKBllsAsync([FromRoute(Name = "BillNumber")] string PK, string currency = "RSD")
        {
            try
            {
                currency = currency.ToUpper();
                var bill = await _BillService.GetByPKAsync(PK, currency);
                var changedCurrency = _exchangeServic.ExchangeBillCurrency(bill, currency);
                return Ok(changedCurrency);
            }
            catch (Exception ex)
            {
                ErrorModel error = new()
                {
                    Error = ex.Message
                };
                return BadRequest(error);
            }
        }

        [HttpDelete]
        [Route("{BillNuber}")]
        public async Task<ActionResult> DeleteBill([FromRoute(Name = "BillNumber")] string PK)
        {
            try
            {
                await _BillService.DeleteBill(PK);
                return Ok();
            }
            catch (Exception e)
            {
                ErrorModel error = new()
                {
                    Error = e.Message
                };
                return NotFound(error);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewBill(CreateBillRequestModel BillToAdd)
        {
            try
            {
                await _BillService.CreateNewBill(BillToAdd);
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
        public async Task<ActionResult> UpdateBill([FromBody]UpdateBillRequest billToUpdate, [FromRoute(Name = "BillNumber")]string PK)
        {
            try
            {
                await _BillService.UpdateBill(billToUpdate, PK);
                return Ok("Updated successfuly!");
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
