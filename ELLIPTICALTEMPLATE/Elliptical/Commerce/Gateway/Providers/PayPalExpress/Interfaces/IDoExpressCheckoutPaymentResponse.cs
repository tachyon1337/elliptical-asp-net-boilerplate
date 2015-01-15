namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    public interface IDoExpressCheckoutPaymentResponse
    {
        PaymentStatus PAYMENTINFO_0_PAYMENTSTATUS { get; set; }
        string PAYMENTINFO_0_PAYMENTTYPE { get; set; }
        string PAYMENTINFO_0_TRANSACTIONID { get; set; }
        string PAYMENTINFO_0_TRANSACTIONTYPE { get; set; }
        string PAYMENTINFO_0_ORDERTIME { get; set; }
        string PAYMENTINFO_0_AMT { get; set; }
        string PAYMENTINFO_0_CURRENCYCODE { get; set; }
        string PAYMENTINFO_0_FEEAMT { get; set; }
        string PAYMENTINFO_0_PENDINGREASON { get; set; }
        string PAYMENTINFO_0_RECEIPTID { get; set; }
    }
}