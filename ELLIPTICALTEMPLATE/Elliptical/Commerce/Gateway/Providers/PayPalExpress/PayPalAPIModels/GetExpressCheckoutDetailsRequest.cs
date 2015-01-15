namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    /// <summary>
    ///     Represents a request for PayPal Payer details for a tokenized transaction request.
    /// </summary>
    public class GetExpressCheckoutDetailsRequest : CommonRequest, IGetExpressCheckoutDetailsRequest
    {
        private readonly string token;

        public GetExpressCheckoutDetailsRequest(string token)
        {
            method = RequestType.GetExpressCheckoutDetails;

            this.token = token;
        }

        public string TOKEN
        {
            get { return token; }
        }
    }
}