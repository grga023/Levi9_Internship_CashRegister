using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Infrastructure.DataModels
{
    public class BillDataModel
    {
        public string BillNumber { get; set; }
        public string PaymentMethod { get; set; }
        public int TotalPrice { get; set; }
        public string CreditCardNumber { get; set; }
    }
}
