using System;
using Elliptical.Mvc.Commerce.Models;

namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    /// <summary>
    ///     TransactionService implements the 3 PayPalExpress apis
    ///     SetExpressCheckout,GetExpressCheckoutDetails,DoExpressCheckoutPayment to negotiate
    ///     a PayPal sale request
    /// </summary>
    public class TransactionService<T> : ITransactionService<T>
    {
        private readonly IPayPalTransaction<T> _payPalTransaction = new PayPalTransaction<T>();

        public SetExpressCheckoutResponse SendPayPalSetExpressCheckoutRequest(PayPalBag bag, IAddress<T> address,
            string serverUrl, string userEmail = null, string paymentDescription = null, string trackingReference = null,
            string currencyCode = "USD", string countryCode = "US")
        {
            try
            {
                Logging.LogMessage("SendPayPalSetExpressCheckoutRequest");

                var shoppingBagService = new ShoppingBagService<T>();
                var expressCheckoutItems = shoppingBagService.LineItems(bag);


                var response = _payPalTransaction.SendSetExpressCheckout(currencyCode, countryCode, paymentDescription,
                    trackingReference, serverUrl, userEmail, bag, address, expressCheckoutItems);

                return response;
            }
            catch (Exception ex)
            {
                Logging.LogException(ex.Message, ex);
            }
            return null;
        }

        public GetExpressCheckoutDetailsResponse SendPayPalGetExpressCheckoutDetailsRequest(string token)
        {
            try
            {
                Logging.LogMessage("SendPayPalGetExpressCheckoutDetailsRequest");
                var response = _payPalTransaction.SendGetExpressCheckoutDetails(token);
               
                return response;
            }
            catch (Exception ex)
            {
                Logging.LogException(ex.Message, ex);
            }
            return null;
        }

        public DoExpressCheckoutPaymentResponse SendPayPalDoExpressCheckoutPaymentRequest(PayPalBag bag, string token,
            string payerId)
        {
            try
            {
                Logging.LogMessage("SendPayPalDoExpressCheckoutPaymentRequest");
                var response = _payPalTransaction.SendDoExpressCheckoutPayment(token, payerId, bag.Currency,
                    bag.TotalAmount, bag.ItemAmount, bag.ShippingAmount, bag.TaxAmount);

                return response;
            }
            catch (Exception ex)
            {
                Logging.LogException(ex.Message, ex);
            }
            return null;
        }
    }
}