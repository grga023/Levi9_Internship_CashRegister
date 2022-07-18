using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Domain.Models
{
    public class ProductBill
    {
        public int ProductQuantity { get; set; }
        public int ProductsPrice { get; set; }

        [ForeignKey("Bill")]
        public string BillNumber { get; set; }
        public Bill Bill { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
