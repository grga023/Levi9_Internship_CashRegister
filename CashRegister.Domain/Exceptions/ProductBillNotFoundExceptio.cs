using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Domain.Exceptions
{
    public class ProductBillNotFoundException : Exception
    {
        public ProductBillNotFoundException() { }
        public ProductBillNotFoundException(string message) : base(message) { }
        public ProductBillNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
