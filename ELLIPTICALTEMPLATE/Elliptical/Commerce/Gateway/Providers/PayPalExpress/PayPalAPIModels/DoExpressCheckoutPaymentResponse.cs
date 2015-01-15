namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    /// <summary>
    ///     Response received from a payment request for a tokenized transaction
    /// </summary>
    public class DoExpressCheckoutPaymentResponse : CommonPaymentResponse, IDoExpressCheckoutPaymentResponse
    {
        // Human Readable re-mapped properties
        public PaymentStatus PaymentStatus
        {
            get { return PAYMENTINFO_0_PAYMENTSTATUS; }
        }

        public string PaymentTransactionId
        {
            get { return PAYMENTINFO_0_TRANSACTIONID; }
        }

        public decimal PaymentAmount
        {
            get { return decimal.Parse(PAYMENTINFO_0_AMT); }
        }

        public decimal PaymentPortionPayPalFees
        {
            get { return decimal.Parse(PAYMENTINFO_0_FEEAMT); }
        }

        public string PaymentReceiptId
        {
            get { return PAYMENTINFO_0_RECEIPTID; }
        }

        public new string ToString
        {
            get
            {
                return
                    string.Format(
                        "Payment Status: [{0}] Payment Type: [{1}] Transaction Type: [{2}] Order Time: [{3}] Currency Code: [{4}] Amount: [{5}] Fees: [{6}] Receipt Id: [{7}]",
                        PaymentStatus, PAYMENTINFO_0_PAYMENTTYPE, PAYMENTINFO_0_TRANSACTIONTYPE, PAYMENTINFO_0_ORDERTIME,
                        PAYMENTINFO_0_CURRENCYCODE, PAYMENTINFO_0_AMT, PAYMENTINFO_0_FEEAMT, PAYMENTINFO_0_RECEIPTID);
            }
        }

        // PayPal Response properties
        public PaymentStatus PAYMENTINFO_0_PAYMENTSTATUS { get; set; }
        public string PAYMENTINFO_0_PAYMENTTYPE { get; set; }
        public string PAYMENTINFO_0_TRANSACTIONID { get; set; }
        public string PAYMENTINFO_0_TRANSACTIONTYPE { get; set; }
        public string PAYMENTINFO_0_ORDERTIME { get; set; }
        public string PAYMENTINFO_0_AMT { get; set; }
        public string PAYMENTINFO_0_CURRENCYCODE { get; set; }
        public string PAYMENTINFO_0_FEEAMT { get; set; }
        public string PAYMENTINFO_0_PENDINGREASON { get; set; } // Can check why PaymentStatus is Pending
        public string PAYMENTINFO_0_RECEIPTID { get; set; }
    }
}