using System.Collections.Generic;
using Elliptical.Mvc.Commerce.Models;

namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    public interface IPayPalTransaction<T>
    {
        /// <summary>
        ///     Setup the Express Checkout request with PayPal
        ///     This sets up the sale for X value in Y currency against a sale description (with optional items)
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <param name="countryCode"></param>
        /// <param name="paymentDescription"></param>
        /// <param name="trackingReference">Unique tracking references for this sale</param>
        /// <param name="serverUrl">Your server URL (Cancel/Return Actions get appended to this)</param>
        /// <param name="userEmail"></param>
        /// <param name="bag"></param>
        /// <param name="address"></param>
        /// <param name="purchaseItems">
        ///     Optional list of individual items being sold in the single payment transaction (note these
        ///     are NOT stored by PayPal against the order)
        /// </param>
        /// <returns></returns>
        SetExpressCheckoutResponse SendSetExpressCheckout(string currencyCode, string countryCode,
            string paymentDescription, string trackingReference, string serverUrl, string userEmail, PayPalBag bag,
            IAddress<T> address, List<ExpressCheckoutItem> purchaseItems = null);

        /// <summary>
        ///     Get PayPal purchase status for the sale and the PayPal account details used for purchase
        /// </summary>
        /// <param name="token">The Express Checkout token for this sale</param>
        /// <returns>GetExpressCheckoutDetailsResponse from PayPal</returns>
        GetExpressCheckoutDetailsResponse SendGetExpressCheckoutDetails(string token);

        /// <summary>
        ///     Request payment to be taken by PayPal for the sale
        /// </summary>
        /// <param name="token"></param>
        /// <param name="payerId"></param>
        /// <param name="currencyCode"></param>
        /// <param name="amount"></param>
        /// <param name="subtotal"></param>
        /// <param name="shipping"></param>
        /// <param name="tax"></param>
        /// <returns>DoExpressCheckoutPaymentResponse from PayPal</returns>
        DoExpressCheckoutPaymentResponse SendDoExpressCheckoutPayment(string token, string payerId, string currencyCode,
            decimal amount, decimal subtotal, decimal shipping, decimal tax);
    }
}