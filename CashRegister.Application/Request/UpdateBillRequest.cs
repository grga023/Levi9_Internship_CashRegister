using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Application.Request
{
    public class UpdateBillRequest
    {
        public string CreditCardNumber { get; set; }
        public string PaymentMethod { get; set; }
    }
}
