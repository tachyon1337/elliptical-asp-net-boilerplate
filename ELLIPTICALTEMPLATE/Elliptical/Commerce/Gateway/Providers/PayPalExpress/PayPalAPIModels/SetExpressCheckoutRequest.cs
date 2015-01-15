using System.Collections.Generic;
using Elliptical.Mvc.Commerce.Models;

namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    /// <summary>
    ///     Represents an initial transaction request that is sent to PayPal.
    ///     Response should return a token and a payerId
    /// </summary>
    public class SetExpressCheckoutRequest<T> : CommonRequest, ISetExpressCheckoutRequest
    {
        private readonly string countryCode;
        private readonly string currencyCode;
        private readonly string email;
        private readonly decimal itemAmount;
        private readonly List<ExpressCheckoutItem> items;
        private readonly PaymentAction paymentAction;
        private readonly string paymentDescription;
        private readonly string serverURL;
        private readonly IAddress<T> shippingAddress;
        private readonly decimal shippingAmount;
        private readonly decimal taxAmount;
        private readonly decimal totalAmount;
        private readonly string trackingReference;

        public SetExpressCheckoutRequest(string currencyCode, string countryCode, string serverURL, string userEmail,
            PayPalBag bag, IAddress<T> address, List<ExpressCheckoutItem> purchaseItems = null,
            string paymentDescription = null, string trackingReference = null)
        {
            method = RequestType.SetExpressCheckout;
            paymentAction = PaymentAction.Sale;
            this.currencyCode = currencyCode;
            this.countryCode = countryCode;
            totalAmount = bag.TotalAmount;
            this.paymentDescription = paymentDescription;
            this.trackingReference = trackingReference;
            this.serverURL = serverURL;
            items = purchaseItems;
            email = userEmail;
            shippingAddress = address;
            shippingAmount = bag.ShippingAmount;
            taxAmount = bag.TaxAmount;
            itemAmount = bag.ItemAmount;
        }

        public string PAYMENTREQUEST_0_PAYMENTACTION
        {
            get { return paymentAction.ToString(); }
        }

        public string PAYMENTREQUEST_0_CURRENCYCODE
        {
            get { return currencyCode; }
        }

        public string PAYMENTREQUEST_0_AMT
        {
            get { return totalAmount.ToString("f2"); }
        }

        [Optional]
        public string PAYMENTREQUEST_0_DESC
        {
            get { return paymentDescription; }
        }

        [Optional]
        public string PAYMENTREQUEST_0_INVNUM
        {
            get { return trackingReference; }
        }

        [Optional]
        public string EMAIL
        {
            get { return email; }
        }

        public string RETURNURL
        {
            get { return serverURL + Configuration.Current.ReturnAction; }
        }

        public string CANCELURL
        {
            get { return serverURL + Configuration.Current.CancelAction; }
        }

        // Optional List of Items in this purchase
        [Optional]
        public List<ExpressCheckoutItem> Items
        {
            get { return items; }
        }

        public string PAYMENTREQUEST_0_ITEMAMT
        {
            get { return itemAmount.ToString("f2"); }
        }

        public string PAYMENTREQUEST_0_SHIPPINGAMT
        {
            get { return shippingAmount.ToString("f2"); }
        }

        public string PAYMENTREQUEST_0_TAXAMT
        {
            get { return taxAmount.ToString("f2"); }
        }

        public string PAYMENTREQUEST_0_SHIPTONAME
        {
            get { return shippingAddress.FirstName + " " + shippingAddress.LastName; }
        }

        public string PAYMENTREQUEST_0_SHIPTOSTREET
        {
            get { return shippingAddress.Street; }
        }

        public string PAYMENTREQUEST_0_SHIPTOCITY
        {
            get { return shippingAddress.City; }
        }

        public string PAYMENTREQUEST_0_SHIPTOSTATE
        {
            get { return shippingAddress.State; }
        }

        public string PAYMENTREQUEST_0_SHIPTOZIP
        {
            get { return shippingAddress.Zip; }
        }

        public string PAYMENTREQUEST_0_SHIPTOCOUNTRYCODE
        {
            get { return countryCode; }
        }

        public string PAYMENTREQUEST_0_SHIPTOPHONENUM
        {
            get { return shippingAddress.Phone; }
        }
    }
}