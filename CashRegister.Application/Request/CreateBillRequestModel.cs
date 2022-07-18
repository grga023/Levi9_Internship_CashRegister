using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;


namespace CashRegister.Application.Request
{
    public class CreateBillRequestModel
    { 
        [Required]
        public string PaymentMethod { get; private set; }
        public string CardNumber { get; private set; }
        [Required]
        public List<CreateBillProductRequestModel> Products { get; private set; }

        public CreateBillRequestModel( string paymentMethod, string cardNumber, List<CreateBillProductRequestModel> products)
        {
            PaymentMethod = paymentMethod;
            CardNumber = cardNumber;
            Products = products;
        }
    }
}
