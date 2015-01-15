namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    public interface IDoExpressCheckoutPaymentRequest
    {
        string TOKEN { get; }
        string PAYERID { get; }
        string PAYMENTREQUEST_0_CURRENCYCODE { get; }
        string PAYMENTREQUEST_0_AMT { get; }
        string PAYMENTREQUEST_0_ITEMAMT { get; }
        string PAYMENTREQUEST_0_SHIPPINGAMT { get; }
        string PAYMENTREQUEST_0_TAXAMT { get; }
        string PAYMENTREQUEST_0_PAYMENTACTION { get; }
    }
}