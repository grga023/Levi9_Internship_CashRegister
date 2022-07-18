using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Domain.Exceptions
{
    public class BillNotFoundException : Exception
    {
        public BillNotFoundException() { }
        public BillNotFoundException(string message) : base(message) { }
        public BillNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
