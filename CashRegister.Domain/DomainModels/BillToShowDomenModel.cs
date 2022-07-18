using CashRegister.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CashRegister.Domain.DomainModels
{
    public class BillToShowDomenModel
    { 
        public string BillNumber { get;  set; }
        public string PaymentMethod { get;  set; }
        public double totalPrice { get;  set; }
        public string Currency { get; set; }    
        public List<ProductBillToShowDomainModel> ProductList { get;  set; } 

    }
}
