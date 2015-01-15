using System.Collections.Generic;
using System.Text;
using System.Web;
using Elliptical.Mvc.Commerce.Models;

namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    /// <summary>
    ///     Default ITransaction implementation
    /// </summary>
    public class PayPalTransaction<T> : IPayPalTransaction<T>
    {
        private readonly Configuration configuration;
        private readonly ResponseSerializer deserializer;
        private readonly IHttpRequestSender requestSender;
        private readonly HttpPostSerializer serializer;

        /// <summary>
        ///     Creates a new instance of the Transaction using the configuration specified in the web.conf, and an HTTP Request
        ///     Sender.
        /// </summary>
        public PayPalTransaction()
            : this(Configuration.Current, new HttpRequestSender())
        {
        }

        /// <summary>
        ///     Creates a new instance of the Transaction
        /// </summary>
        public PayPalTransaction(Configuration configuration, IHttpRequestSender requestSender)
        {
            this.configuration = configuration;
            this.requestSender = requestSender;
            serializer = new HttpPostSerializer();
            deserializer = new ResponseSerializer();
        }

        public SetExpressCheckoutResponse SendSetExpressCheckout(string currencyCode, string countryCode,
            string paymentDescription, string trackingReference, string serverURL, string userEmail, PayPalBag bag,
            IAddress<T> address, List<ExpressCheckoutItem> purchaseItems = null)
        {
            var request = new SetExpressCheckoutRequest<T>(currencyCode, countryCode, serverURL, userEmail, bag, address,
                purchaseItems, paymentDescription, trackingReference);

            var postData = serializer.Serialize(request);
            Logging.LogLongMessage("PayPal Send Request", "Serlialized Request to PayPal API: " + postData);

            var response = requestSender.SendRequest(Configuration.Current.PayPalAPIUrl, postData);
            var decodedResponse = HttpUtility.UrlDecode(response, Encoding.Default);
            Logging.LogLongMessage("PayPal Response Received", "Decoded Respose from PayPal API: " + decodedResponse);

            return deserializer.Deserialize<SetExpressCheckoutResponse>(decodedResponse);
        }

        public GetExpressCheckoutDetailsResponse SendGetExpressCheckoutDetails(string token)
        {
            var request = new GetExpressCheckoutDetailsRequest(token);

            var postData = serializer.Serialize(request);
            Logging.LogLongMessage("PayPal Send Request", "Serlialized Request to PayPal API: " + postData);

            var response = requestSender.SendRequest(Configuration.Current.PayPalAPIUrl, postData);
            var decodedResponse = HttpUtility.UrlDecode(response, Encoding.Default);
            Logging.LogLongMessage("PayPal Response Recieved", "Decoded Respose from PayPal API: " + decodedResponse);

            return deserializer.Deserialize<GetExpressCheckoutDetailsResponse>(decodedResponse);
        }

        public DoExpressCheckoutPaymentResponse SendDoExpressCheckoutPayment(string token, string payerId,
            string currencyCode, decimal amount, decimal subtotal, decimal shipping, decimal tax)
        {
            var request = new DoExpressCheckoutPaymentRequest(token, payerId, currencyCode, amount, subtotal, shipping,
                tax);

            var postData = serializer.Serialize(request);
            Logging.LogLongMessage("PayPal Send Request", "Serlialized Request to PayPal API: " + postData);

            var response = requestSender.SendRequest(Configuration.Current.PayPalAPIUrl, postData);
            var decodedResponse = HttpUtility.UrlDecode(response, Encoding.Default);
            Logging.LogLongMessage("PayPal Response Recieved", "Decoded Respose from PayPal API: " + decodedResponse);

            return deserializer.Deserialize<DoExpressCheckoutPaymentResponse>(decodedResponse);
        }
    }
}