using System;
using System.Collections.Generic;
using Elliptical.Mvc.Commerce.Models;
using Elliptical.Mvc.Configuration.Commerce;

namespace Elliptical.Mvc.Commerce.Gateway
{
    /// <summary>
    ///     Implements both service pattern and dependency injection pattern for resolving payment gateway provider
    ///     and decrypted credit card details at run time
    /// </summary>
    public class Client<T> : IClient<T>
    {
        public BillingType BillingType = BillingType.Single;
        public TransactionType TransactionType = TransactionType.Charge;
        private readonly ICardTransaction<T> _cardTransaction;
        private readonly string _cardType;
        private readonly CreditCardType _creditCardType;
        private readonly ITransaction<T> _transaction;

        /// <summary>
        ///     constructor
        /// </summary>
        /// <param name="transaction"></param>
        public Client(ITransaction<T> transaction)
        {
            _transaction = transaction;
            var cardType = transaction.CreditCardItem.Id;
            _cardType = cardType;
            _creditCardType = ApplicationCreditCard.GetCreditCardType(cardType);
        }

        /// <summary>
        ///     overloaded constructor, the overload param cardTransaction is dependency-injected ICardTransaction implementation
        ///     for
        ///     returning decrypted card details
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="cardTransaction"></param>
        public Client(ITransaction<T> transaction, ICardTransaction<T> cardTransaction)
        {
            _transaction = transaction;
            _cardTransaction = cardTransaction;
            var cardType = transaction.CreditCardItem.Id;
            _cardType = cardType;
            _creditCardType = ApplicationCreditCard.GetCreditCardType(cardType);
        }

        private string Mode
        {
            get { return Configuration.GetMode(); }
        }

        /// <summary>
        ///     implements a provider locator pattern, returning an instance of the correct provider type responsible for
        ///     processing
        ///     a payment type in the current gateway mode.
        ///     Relies on a dependency-injected implementation of ICardTransaction to populate card details
        /// </summary>
        /// <returns></returns>
        public IPaymentGateway<T> resolveType()
        {
            var cardType = _cardType;
            var mode = Mode;
            var provider = getProvider(mode, cardType);
            var settings = Configuration.GetProviderSettings(provider);
            var typeName = provider.Type;
            var t = Type.GetType(typeName);
            var providerType = (IPaymentGateway<T>) Activator.CreateInstance(t);
            providerType.Settings = settings;
            providerType.Mode = mode;

            var transaction = _transaction;
            var address = transaction.BillingAddress;
            var shoppingBag = transaction.ShoppingBag;
            var creditCardItem = transaction.CreditCardItem;
            var paymentItem = transaction.PaymentItem;
            var submitLastTransactionCard = transaction.SubmitLastTransactionCard;

            var cardNumber = transaction.CreditCardItem.CardNumber;
            var ccCode = transaction.CreditCardItem.CVVCode;
            var userId = shoppingBag.Id;
            providerType.Address = address.Street;
            providerType.Amount = shoppingBag.Total;
            providerType.CardType = _creditCardType;
            providerType.sCardType = _cardType;
            providerType.City = address.City;
            providerType.Country = "";
            providerType.FirstName = address.FirstName;
            providerType.LastName = address.LastName;
            providerType.Mode = mode;
            providerType.Month = creditCardItem.ExpMonth;
            providerType.StateProvince = address.State;
            providerType.Phone = address.Phone;
            providerType.Year = creditCardItem.ExpYear;
            providerType.ZipPostal = address.Zip;
            providerType.UserId = userId;
            providerType.Transaction = transaction;
            providerType.ResponseFields = new Dictionary<string, string>();
            providerType.RequestFields = new Dictionary<string, string>();

            var cardTransaction = _cardTransaction;
            if (submitLastTransactionCard && cardTransaction != null)
            {
                cardTransaction.UserId = userId;
                cardTransaction.CardItem = creditCardItem;
                var item = cardTransaction.GetCard();
                providerType.CardNumber = item.CardNumber;
                providerType.CCCode = item.CVVCode;
                providerType.Month = item.ExpMonth;
                providerType.Year = item.ExpYear;
            }
            else
            {
                providerType.CardNumber = cardNumber;
                providerType.CCCode = ccCode;
            }
            if (providerType.Validate)
            {
            }
            var reqFields = setRequestFields(paymentItem);
            providerType.RequestFields = reqFields;

            return providerType;
        }

        /// <summary>
        ///     returns  the gateway provider that processes a cardType in the current gateway mode
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="cardType"></param>
        /// <returns></returns>
        private Provider getProvider(string mode, string cardType)
        {
            var separator = ',';
            Provider provider = null;
            var providers = Configuration.GetActiveModeProviders(mode);
            foreach (var p in providers)
            {
                var cardTypes = p.ProcessedPaymentTypes;
                var types = cardTypes.Split(separator);
                if (handlesPaymentType(types, cardType))
                {
                    provider = p;
                    break;
                }
            }
            if (provider != null)
            {
                return provider;
            }
            throw new Exception("A payment gateway provider for the submitted payment type is required.");
        }

        /// <summary>
        ///     converts a PaymentItem object into a RequestFields Dictionary
        /// </summary>
        /// <param name="paymentItem"></param>
        /// <returns></returns>
        private Dictionary<string, string> setRequestFields(PaymentItem<T> paymentItem)
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("CardType", paymentItem.CardType);
            if (paymentItem.PayerId != null)
            {
                dictionary.Add("PayerId", paymentItem.PayerId);
            }
            if (paymentItem.Token != null)
            {
                dictionary.Add("Token", paymentItem.Token);
            }
            return dictionary;
        }

        /// <summary>
        ///     Tests if a Gateway Provider is configured to process a cardType
        /// </summary>
        /// <param name="types"></param>
        /// <param name="cardType"></param>
        /// <returns></returns>
        private bool handlesPaymentType(string[] types, string cardType)
        {
            var handles = false;
            var any = "any";
            foreach (var s in types)
            {
                if (s.ToLower() == cardType.ToLower() || s.ToLower() == any)
                {
                    handles = true;
                    break;
                }
            }

            return handles;
        }
    }
}