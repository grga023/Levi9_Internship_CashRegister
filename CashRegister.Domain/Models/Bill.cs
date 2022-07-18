using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Domain.Models
{
    public class Bill
    {
        [Key]
        public string BillNumber { get; set; }
        public string PaymentMethod { get; set; }
        public int TotalPrice { get; set; }
        public string CreditCardNumber { get; set; }
        public List<ProductBill> ProductBills { get; set; }
    }
}
