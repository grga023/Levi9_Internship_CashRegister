using CashRegister.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Domain.DomainModels
{
    public class ProductBillToShowDomainModel
    {
        public int Quantity { get;  set; }
        public double ProductsPrice { get; set; }
        public string ProductName { get;  set; }
        public double ProductPrice { get;  set; }
        
    }
}
