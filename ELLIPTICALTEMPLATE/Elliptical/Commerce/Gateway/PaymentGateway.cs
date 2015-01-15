using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Elliptical.Mvc.Commerce.Models;

namespace Elliptical.Mvc.Commerce.Gateway
{
    /// <summary>
    ///     Base class implementation of IPaymentGateway
    /// </summary>
    public class PaymentGateway<T> : IPaymentGateway<T>
    {
        public PaymentGateway()
        {
            Validate = true;
            Log = false;
        }

        public BillingType BillingType { get; set; }
        public ITransaction<T> Transaction { get; set; }
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        public string CardProvider { get; set; }
        public string CCCode { get; set; }
        public string sCardType { get; set; }
        public CreditCardType CardType { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string Country { get; set; }
        public string ZipPostal { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string TransactionKey { get; set; }
        public string AuthCode { get; set; }
        public string TransactionId { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string LogMessage { get; set; }
        public string Mode { get; set; }
        public string Phone { get; set; }
        public object UserId { get; set; }
        public bool Validate { get; set; }
        public bool Log { get; set; }
        public TransactionType TransactionType { get; set; }
        public NameValueCollection Settings { get; set; }
        public Dictionary<string, string> RequestFields { get; set; }
        public Dictionary<string, string> ResponseFields { get; set; }
        public object PaymentTransaction { get; set; }

        public virtual TransactionResult charge()
        {
            throw new NotImplementedException();
        }

        protected internal bool validateCardType()
        {
            var cardType = sCardType;
            return ApplicationCreditCard.IsValidCreditCardType(cardType);
        }

        private bool validateCardNumber()
        {
            var cardNumber = CardNumber;
            return ApplicationCreditCard.Validate(cardNumber);
        }

        private bool validateCardExpDate()
        {
            var date = Month + "/" + Year;
            return ApplicationCreditCard.ValidateExpiryDate(date);
        }
    }
}