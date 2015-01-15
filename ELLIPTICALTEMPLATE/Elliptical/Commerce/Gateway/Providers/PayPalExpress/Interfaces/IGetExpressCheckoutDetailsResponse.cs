namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    public interface IGetExpressCheckoutDetailsResponse
    {
        CheckoutStatus CHECKOUTSTATUS { get; set; }
        string PAYMENTREQUEST_0_INVNUM { get; set; }
        string PAYERSTATUS { get; set; }
        string PAYERID { get; set; }
        string EMAIL { get; set; }
        string SALUTATION { get; set; }
        string FIRSTNAME { get; set; }
        string MIDDLENAME { get; set; }
        string LASTNAME { get; set; }
    }
}