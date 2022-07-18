using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;

namespace CashRegister.UnitTest.Bills
{
    [TestFixture]
    public class BillServiceTest
    {
        [Test]
        public void DeleteBill_WhenCalled_DeleteBillFromDb()
        {
            var register = new Mock<IBillRepository>();
        }
    }
}
