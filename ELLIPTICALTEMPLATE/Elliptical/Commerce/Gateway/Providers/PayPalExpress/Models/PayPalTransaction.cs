using System;

namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    public class PayPalTransaction
    {
        public string OrderId { get; set; }
        public string RequestId { get; set; }
        public string TrackingReference { get; set; }
        public DateTime RequestTime { get; set; }
        public string RequestStatus { get; set; }
        public string TimeStamp { get; set; }
        public string RequestError { get; set; }
        public string Token { get; set; }
        public string RequestData { get; set; }
        public string PaymentTransactionId { get; set; }
        public string PaymentAuthCode { get; set; }
        public string PaymentError { get; set; }
        public string UserId { get; set; }
    }
}