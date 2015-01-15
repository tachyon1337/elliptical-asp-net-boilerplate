namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    /// <summary>
    ///     Represents a payment request to PayPal for a tokenized transaction request.
    /// </summary>
    public class DoExpressCheckoutPaymentRequest : CommonRequest, IDoExpressCheckoutPaymentRequest
    {
        private readonly decimal amount;
        private readonly string currencyCode;
        private readonly string payerId;
        private readonly PaymentAction paymentAction;
        private readonly decimal shipping;
        private readonly decimal subtotal;
        private readonly decimal tax;
        private readonly string token;

        public DoExpressCheckoutPaymentRequest(string token, string payerId, string currencyCode, decimal amount,
            decimal subtotal, decimal shipping, decimal tax)
        {
            method = RequestType.DoExpressCheckoutPayment;
            paymentAction = PaymentAction.Sale;

            this.token = token;
            this.payerId = payerId;

            this.currencyCode = currencyCode;
            this.amount = amount;
            this.subtotal = subtotal;
            this.shipping = shipping;
            this.tax = tax;
        }

        public string TOKEN
        {
            get { return token; }
        }

        public string PAYERID
        {
            get { return payerId; }
        }

        public string PAYMENTREQUEST_0_CURRENCYCODE
        {
            get { return currencyCode; }
        }

        public string PAYMENTREQUEST_0_AMT
        {
            get { return amount.ToString("f2"); }
        }

        public string PAYMENTREQUEST_0_ITEMAMT
        {
            get { return subtotal.ToString("f2"); }
        }

        public string PAYMENTREQUEST_0_SHIPPINGAMT
        {
            get { return shipping.ToString("f2"); }
        }

        public string PAYMENTREQUEST_0_TAXAMT
        {
            get { return tax.ToString("f2"); }
        }

        public string PAYMENTREQUEST_0_PAYMENTACTION
        {
            get { return paymentAction.ToString(); }
        }
    }
}