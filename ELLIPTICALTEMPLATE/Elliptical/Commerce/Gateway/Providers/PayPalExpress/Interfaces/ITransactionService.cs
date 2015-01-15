using Elliptical.Mvc.Commerce.Models;

namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    public interface ITransactionService<T>
    {
        SetExpressCheckoutResponse SendPayPalSetExpressCheckoutRequest(PayPalBag bag, IAddress<T> address,
            string serverUrl, string userEmail = null, string paymentDescription = null, string trackingReference = null,
            string currencyCode = "USD", string countryCode = "US");

        GetExpressCheckoutDetailsResponse SendPayPalGetExpressCheckoutDetailsRequest(string token);

        DoExpressCheckoutPaymentResponse SendPayPalDoExpressCheckoutPaymentRequest(PayPalBag bag, string token,
            string payerId);
    }
}