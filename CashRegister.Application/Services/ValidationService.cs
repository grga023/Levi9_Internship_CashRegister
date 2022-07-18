using CashRegister.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Application.Services
{
    public class ValidationService : IValidationService
    {
        public bool IsValidBillNumber(string billNumber)
        {
            billNumber = billNumber.Replace("-", "");

            if (billNumber.Length < 18)
            {
                return false;
            }
            int controlNumber = Convert.ToInt32(billNumber.Substring(billNumber.Length - 2));
            string billSubstring = billNumber.Substring(0, 16);
            long numberBody = long.Parse(billSubstring);
            if (98 - ((numberBody * 100) % 97) == controlNumber)
            {
                return true;
            }
            return false;
        }
        public bool isValidCreditCard(string creditCard)
        {
            bool isValid = true;
            if (creditCard == null)
            {
                isValid = false;
                return isValid;
            }
            if (creditCard.Length != 13 && creditCard.Length != 15 && creditCard.Length != 16)
            {
                isValid = false;
            }
            else
            {
                if ((creditCard.Length == 13 || creditCard.Length == 16) && creditCard.StartsWith('4'))
                {
                    isValid = ValidateCreditCard(creditCard);

                }
                else if (creditCard.Length == 15 && (creditCard.StartsWith("34") || creditCard.StartsWith("37")))
                {
                    isValid = ValidateCreditCard(creditCard);
                }
                else if (creditCard.Length == 16 && (creditCard.StartsWith("51") || creditCard.StartsWith("52") || creditCard.StartsWith("53")
                    || creditCard.StartsWith("54") || creditCard.StartsWith("55")))
                {
                    isValid = ValidateCreditCard(creditCard);
                }

                else isValid = false;
            }
            return isValid;
        }
        private bool ValidateCreditCard(string card)
        {
            var cardReverse = card.Reverse();
            var reverseEveryOtherSecondToLast = new string(cardReverse.Where((ch, index) => index % 2 != 0).ToArray());
            string MultiplyDigitsByTwo = "";
            for (int i = 0; i < reverseEveryOtherSecondToLast.Length; i++)
            {
                int num = Int32.Parse(reverseEveryOtherSecondToLast[i].ToString());
                num = num * 2;
                MultiplyDigitsByTwo = MultiplyDigitsByTwo + num.ToString();
            }
            int multipliedDigitsSummed = 0;
            for (int i = 0; i < MultiplyDigitsByTwo.Length; i++)
            {
                int num = Int32.Parse(MultiplyDigitsByTwo[i].ToString());
                multipliedDigitsSummed = multipliedDigitsSummed + num;
            }
            var digitsWerentMultiplied = new string(cardReverse.Where((ch, index) => index % 2 == 0).ToArray());
            var digitsWerentMultipliedToDigits = Int32.Parse(digitsWerentMultiplied);
            int result = 0;
            for (int i = 0; i < digitsWerentMultiplied.Length; i++)
            {
                int num = Int32.Parse((digitsWerentMultiplied[i].ToString()));
                result = result + num;
            }
            int endResult = result + multipliedDigitsSummed;
            bool isValidCard = endResult % 10 == 0;
            return isValidCard;
        }
    }
}
