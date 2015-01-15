namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    /// <summary>
    ///     Returns Payer details for a payer who has authorized the PayPal payment for a tokenized transaction request
    /// </summary>
    public class GetExpressCheckoutDetailsResponse : CommonPaymentResponse, IGetExpressCheckoutDetailsResponse
    {
        // Human Readable re-mapped properties
        public string TrackingReference
        {
            get { return PAYMENTREQUEST_0_INVNUM; }
        }

        public new string ToString
        {
            get
            {
                return
                    string.Format(
                        "Checkout Status: [{0}] Payer Status: [{1}] Name: [{2} {3} {4} {5}] PayPal Email: [{6}]",
                        CHECKOUTSTATUS, PAYERSTATUS, SALUTATION, FIRSTNAME, MIDDLENAME, LASTNAME, EMAIL);
            }
        }

        // PayPal Response properties
        public CheckoutStatus CHECKOUTSTATUS { get; set; }
        public string PAYMENTREQUEST_0_INVNUM { get; set; }
        public string PAYERSTATUS { get; set; }
        public string PAYERID { get; set; }
        public string EMAIL { get; set; }
        public string SALUTATION { get; set; }
        public string FIRSTNAME { get; set; }
        public string MIDDLENAME { get; set; }
        public string LASTNAME { get; set; }
    }
}