namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    /// <summary>
    ///     Response received from a SetExpressCheckout transaction request
    /// </summary>
    public class SetExpressCheckoutResponse : CommonResponse, ISetExpressCheckoutResponse
    {
        /// <summary>
        ///     Transaction Token
        /// </summary>
        public string TOKEN { get; set; }
    }
}