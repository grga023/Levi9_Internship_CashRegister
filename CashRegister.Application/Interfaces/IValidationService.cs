using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Application.Interfaces
{
    public interface IValidationService
    {
        public bool IsValidBillNumber(string billNumber);
        public bool isValidCreditCard(string creditCard);
    }
}
