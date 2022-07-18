using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Application.Request
{
    public class CreateProductRequest
    {
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
    }
}
