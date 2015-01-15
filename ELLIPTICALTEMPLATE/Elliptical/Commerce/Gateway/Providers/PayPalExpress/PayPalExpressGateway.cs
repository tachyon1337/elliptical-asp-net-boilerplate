using System;

namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    public class PayPalExpressGateway<T> : PaymentGateway<T>
    {
        private const string PAYMENTINFO_0_RECEIPTID = "PAYMENTINFO_0_RECEIPTID";
        private const string PAYMENTINFO_0_PAYMENTSTATUS = "PAYMENTINFO_0_PAYMENTSTATUS";
        private const string PAYMENTINFO_0_PAYMENTTYPE = "PAYMENTINFO_0_PAYMENTTYPE";
        private const string PAYMENTINFO_0_TRANSACTIONID = "PAYMENTINFO_0_TRANSACTIONID";
        private const string PAYMENTINFO_0_TRANSACTIONTYPE = "PAYMENTINFO_0_TRANSACTIONTYPE";
        private const string PAYMENTINFO_0_ORDERTIME = "PAYMENTINFO_0_ORDERTIME";
        private const string PAYMENTINFO_0_AMT = "PAYMENTINFO_0_AMT";
        private const string PAYMENTINFO_0_CURRENCYCODE = "PAYMENTINFO_0_CURRENCYCODE";
        private const string PAYMENTINFO_0_FEEAMT = "PAYMENTINFO_0_FEEAMT";
        private const string PAYMENTINFO_0_PENDINGREASON = "PAYMENTINFO_0_PENDINGREASON";

        public override TransactionResult charge()
        {
            var requestFields = RequestFields;
            var token = requestFields["Token"];
            var payerId = requestFields["PayerId"];

            var shoppingBag = Transaction.ShoppingBag;
            var paymentItem = Transaction.PaymentItem;
            var invoiceNo = paymentItem.InvoiceNo;
            var paymentTransactionId = "";
            var bagService = new ShoppingBagService<T>();
            var bag = bagService.GetPayPalBag(shoppingBag);
            var transactionService = new TransactionService<T>();
            var response = transactionService.SendPayPalDoExpressCheckoutPaymentRequest(bag, token, payerId);

            var errorMessage = "";
            //add response fields to ResponseFields dictionary
            ResponseFields.Add(PAYMENTINFO_0_RECEIPTID, response.PAYMENTINFO_0_RECEIPTID);
            ResponseFields.Add(PAYMENTINFO_0_AMT, response.PAYMENTINFO_0_AMT);
            ResponseFields.Add(PAYMENTINFO_0_CURRENCYCODE, response.PAYMENTINFO_0_CURRENCYCODE);
            ResponseFields.Add(PAYMENTINFO_0_FEEAMT, response.PAYMENTINFO_0_FEEAMT);
            ResponseFields.Add(PAYMENTINFO_0_ORDERTIME, response.PAYMENTINFO_0_ORDERTIME);
            ResponseFields.Add(PAYMENTINFO_0_PAYMENTSTATUS, response.PAYMENTINFO_0_PAYMENTSTATUS.ToString());
            ResponseFields.Add(PAYMENTINFO_0_PAYMENTTYPE, response.PAYMENTINFO_0_PAYMENTTYPE);
            ResponseFields.Add(PAYMENTINFO_0_PENDINGREASON, response.PAYMENTINFO_0_PENDINGREASON);
            ResponseFields.Add(PAYMENTINFO_0_TRANSACTIONID, response.PAYMENTINFO_0_TRANSACTIONID);
            ResponseFields.Add(PAYMENTINFO_0_TRANSACTIONTYPE, response.PAYMENTINFO_0_TRANSACTIONTYPE);

            if (response == null || response.ResponseStatus != ResponseType.Success)
            {
                errorMessage = (response == null) ? "Null Transaction Response" : response.ErrorToString;
                ErrorMessage = errorMessage;
                return TransactionResult.Declined;
            }
            if (response.PaymentStatus == PaymentStatus.Completed)
            {
                // Add a PayPal transaction object
                paymentTransactionId = response.PaymentReceiptId;
                if (paymentTransactionId == "" || paymentTransactionId == null)
                {
                    paymentTransactionId = invoiceNo;
                }
                var transaction = new PayPalTransaction
                {
                    OrderId = null,
                    RequestId = payerId,
                    TrackingReference = invoiceNo,
                    RequestTime = DateTime.Now,
                    RequestStatus = response.ResponseStatus.ToString(),
                    TimeStamp = response.TIMESTAMP,
                    RequestError = response.ErrorToString,
                    Token = response.TOKEN,
                    RequestData = response.ToString,
                    PaymentTransactionId = paymentTransactionId,
                    PaymentAuthCode = response.PaymentTransactionId,
                    PaymentError = response.PaymentErrorToString,
                    UserId = bag.Id.ToString()
                };

                AuthCode = response.PaymentTransactionId;
                TransactionId = paymentTransactionId;
                LogMessage = response.ToString;
                CardProvider = "PayPal";
                CardNumber = "PayPal";
                CCCode = "";
                Month = 0;
                Year = 0;
                PaymentTransaction = transaction;

                return TransactionResult.Approved;
            }
            ErrorMessage = "Error taking PayPal payment. Error: " + response.ErrorToString + " - Payment Error: " +
                           response.PaymentErrorToString;
            var msg = response.PAYMENTREQUEST_0_LONGMESSAGE;
            ResponseFields.Add("PAYMENTREQUEST_0_LONGMESSAGE", msg);
            return TransactionResult.Declined;
        }
    }
}