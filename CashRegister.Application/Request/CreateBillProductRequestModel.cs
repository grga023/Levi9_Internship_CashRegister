using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CashRegister.Application.Request
{
    public class CreateBillProductRequestModel
    {
        [Required]
        [Range(1,99999)]
        public int Quantity { get; set; }
        [Required]
        public int ProductId { get; set; }

        public CreateBillProductRequestModel(int quantity, int productId)
        {
            Quantity = quantity;
            ProductId = productId;
        }
    }
}
