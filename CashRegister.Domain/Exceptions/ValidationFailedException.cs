using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Domain.Exceptions
{
    public class ValidationFailedException : Exception
    {
        public ValidationFailedException() { }
        public ValidationFailedException(string message) : base(message) { }
        public ValidationFailedException(string message, Exception inner) : base(message, inner) { }
    }
}
