using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using Elliptical.Mvc.Commerce.Models;
using Elliptical.Mvc.Configuration.Commerce;

namespace Elliptical.Mvc.Commerce
{
    public static class ApplicationCreditCard
    {
        private static string _encryptionKey;

        private static readonly CommerceSection config =
            ConfigurationManager.GetSection("ellipticalCommerce") as CommerceSection;

        /// <summary>
        ///     get list of accepted credit cards
        /// </summary>
        /// <returns></returns>
        public static List<CreditCard> GetAcceptedCreditCards()
        {
            var creditCards = new List<CreditCard>();
            var acceptedCards = config.CreditCardConfiguration.AcceptedCards.OfType<Card>();

            foreach (var card in acceptedCards)
            {
                var creditCard = new CreditCard {RedirectAction = card.RedirectAction, Id = card.Id, Label = card.Name};

                creditCards.Add(creditCard);
            }

            return creditCards;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static bool AllowLastCreditCardForTransaction()
        {
            var creditCard = config.CreditCardConfiguration;
            var allow = false;
            if (creditCard.GetType().GetProperty("EncryptLastTransaction") != null)
            {
                allow = creditCard.EncryptLastTransaction;
            }

            return allow;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static string GetEncryptionKey()
        {
            if (_encryptionKey != null)
            {
                return _encryptionKey;
            }
            string key = null;
            var creditCard = config.CreditCardConfiguration;
            if (creditCard.GetType().GetProperty("EncryptionKey") != null)
            {
                key = creditCard.EncryptionKey;
            }
            _encryptionKey = key;
            return key;
        }

        /// <summary>
        ///     returns an encoded credit card string in Last-4-Digits format <CardType xxxx-xxxx-xxxx-1234>
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="cardType"></param>
        /// <returns></returns>
        public static string EncodedCreditCard(string cardNumber, string cardType)
        {
            var cardType_ = GetCreditCardType(cardType);
            if (cardNumber.Length == 4)
            {
                return encodeCreditCardText(cardNumber, cardType_);
            }
            return encodeCreditCardText(cardNumber, cardType_, true);
        }

        /// <summary>
        ///     overload method that returns an encoded credit card string in Last-4-Digits format <CardType xxxx-xxxx-xxxx-1234>
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="cardType"></param>
        /// <returns></returns>
        public static string EncodedCreditCard(string cardNumber, CreditCardType cardType)
        {
            if (cardNumber.Length == 4)
            {
                return encodeCreditCardText(cardNumber, cardType);
            }
            return encodeCreditCardText(cardNumber, cardType, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public static string Encrypt(string cardNumber)
        {
            if (!Validate(cardNumber))
            {
                throw new Exception("Error: Cannot encrypt invalid credit card number");
            }
            var key = GetEncryptionKey();
            var crypto = new Crypto(key);
            return crypto.Encrypt(cardNumber);
        }

        /// <summary>
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public static string Decrypt(string cardNumber)
        {
            if (Validate(cardNumber))
            {
                return cardNumber;
            }
            var key = GetEncryptionKey();
            var crypto = new Crypto(key);
            return crypto.Decrypt(cardNumber);
        }

        /// <summary>
        ///     returns a CreditCardType enumerated list value
        /// </summary>
        /// <param name="cardType"></param>
        /// <returns></returns>
        public static CreditCardType GetCreditCardType(string cardType)
        {
            cardType = cardType.ToLower();
            if (cardType == "mastercard")
            {
                return CreditCardType.MasterCard;
            }
            if (cardType == "american express")
            {
                return CreditCardType.Amex;
            }
            if (cardType == "amex")
            {
                return CreditCardType.Amex;
            }
            if (cardType == "visa")
            {
                return CreditCardType.Visa;
            }
            if (cardType == "discover")
            {
                return CreditCardType.Discover;
            }
            if (cardType == "paypal")
            {
                return CreditCardType.PayPal;
            }
            if (cardType == "dinersclub")
            {
                return CreditCardType.DinersClub;
            }
            if (cardType == "jcb")
            {
                return CreditCardType.JCB;
            }
            if (cardType == "enroute")
            {
                return CreditCardType.enRoute;
            }
            if (cardType == "solo")
            {
                return CreditCardType.Solo;
            }
            if (cardType == "switch")
            {
                return CreditCardType.Switch;
            }
            if (cardType == "maestro")
            {
                return CreditCardType.Maestro;
            }
            if (cardType == "visaelectron")
            {
                return CreditCardType.VisaElectron;
            }
            if (cardType == "lasercard")
            {
                return CreditCardType.LaserCard;
            }
            return CreditCardType.Invalid;
        }

        /// <summary>
        /// </summary>
        /// <param name="cardType"></param>
        /// <returns></returns>
        public static string ToValidCreditCardTypeString(this string cardType)
        {
            cardType = cardType.ToLower();
            switch (cardType)
            {
                case "american express":
                    return "Amex";

                case "amex":
                    return "Amex";

                case "master card":
                    return "MasterCard";

                case "mastercard":
                    return "MasterCard";

                case "visa":
                    return "Visa";

                case "discover":
                    return "Discover";

                case "paypal":
                    return "PayPal";

                case "dinersclub":
                    return "DinersClub";

                case "diners club":
                    return "DinersClub";

                case "jcb":
                    return "JCB";

                case "enroute":
                    return "enRoute";

                case "solo":
                    return "Solo";

                case "switch":
                    return "Switch";

                case "maestro":
                    return "maestro";

                case "visaelectron":
                    return "VisaElectron";

                case "visa electron":
                    return "VisaElectron";

                case "lasercard":
                    return "LaserCard";

                case "laser card":
                    return "LaserCard";

                case "bitcoin":
                    return "Bitcoin";

                default:
                    return "Invalid";
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="cardType"></param>
        /// <returns></returns>
        public static bool IsValidCreditCardType(string cardType)
        {
            try
            {
                var creditCardType = (CreditCardType) Enum.Parse(typeof (CreditCardType), cardType, true);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public static bool Validate(string cardNumber)
        {
            //// check whether input string is null or empty, or invalid length(encrypted format)
            if (string.IsNullOrEmpty(cardNumber))
            {
                return false;
            }
            cardNumber.Replace(" ", "");
            cardNumber.Replace("-", "");
            if (!cardNumber.IsNumeric())
            {
                return false;
            }
            int[] DELTAS = {0, 1, 2, 3, 4, -4, -3, -2, -1, 0};
            var checksum = 0;
            var chars = cardNumber.ToCharArray();
            for (var i = chars.Length - 1; i > -1; i--)
            {
                var j = chars[i] - 48;
                checksum += j;
                if (((i - chars.Length)%2) == 0)
                    checksum += DELTAS[j];
            }

            return ((checksum%10) == 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="expiryDate"></param>
        /// <returns></returns>
        public static bool ValidateExpiryDate(string expiryDate)
        {
            //@"^((0[1-9])|(1[0-2]))\/((2009)|(20[1-2][0-9]))$" - 2010-2029
            //"^((0[1-9])|(1[0-2]))\/(\d{4})$" - any year
            var match = Regex.Match(expiryDate, @"^((0[1-9])|(1[0-2]))\/((2009)|(20[1-2][0-9]))$");

            if (!match.Success)
                return false;

            var year = int.Parse(expiryDate.Substring(3));
            var month = int.Parse(expiryDate.Substring(0, 2));
            var exp = new DateTime(year, month, 1);
            var currMonth = DateTime.Now;
            if (exp > currMonth)
                return false;

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private static double normalizeYear(double year)
        {
            var YEARS_AHEAD = 20;
            if (year < 100)
            {
                double nowYear = DateTime.Now.Year;
                year += Math.Floor(nowYear/100)*100;
                if (year > nowYear + YEARS_AHEAD)
                {
                    year -= 100;
                }
                else if (year <= nowYear - 100 + YEARS_AHEAD)
                {
                    year += 100;
                }
            }
            return year;
        }

        /// <summary>
        ///     returns last 4 digit encoded credit card number
        /// </summary>
        /// <param name="last4Digits"></param>
        /// <param name="cardType"></param>
        /// <returns></returns>
        private static string encodeCreditCardText(string last4Digits, CreditCardType cardType)
        {
            try
            {
                var encodedText = "";
                if (cardType == CreditCardType.Amex)
                {
                    encodedText = amexString();
                }
                else
                {
                    encodedText = bankString();
                }

                encodedText += last4Digits;
                return encodedText;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        ///     overload method: returns last 4 digit encoded credit card number from plaintext card number or encrypted card
        ///     number
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="cardType"></param>
        /// <param name="isEncrypted"></param>
        /// <returns></returns>
        private static string encodeCreditCardText(string cardNumber, CreditCardType cardType, bool isEncrypted)
        {
            try
            {
                var encodedText = "";
                var last4Digits = "";
                if (cardType == CreditCardType.Amex || cardType == CreditCardType.enRoute)
                {
                    encodedText = amexString();
                }
                else if (cardType == CreditCardType.CarteBlanche || cardType == CreditCardType.DinersClub)
                {
                    encodedText = cardString();
                }
                else
                {
                    encodedText = bankString();
                }
                if (!isEncrypted)
                {
                    last4Digits = cardNumber.LastChars(4);
                    encodedText += last4Digits;
                    return encodedText;
                }
                var key = GetEncryptionKey();
                var crypto = new Crypto(key);
                var decryptedCardNumber = crypto.Decrypt(cardNumber);
                last4Digits = cardNumber.LastChars(4);
                encodedText += last4Digits;
                return encodedText;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private static string bankString()
        {
            return "xxxx-xxxx-xxxx-";
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private static string amexString()
        {
            return "xxxx-xxxxxx-x";
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private static string cardString()
        {
            return "xxxx-xxxxxx";
        }
    }
}