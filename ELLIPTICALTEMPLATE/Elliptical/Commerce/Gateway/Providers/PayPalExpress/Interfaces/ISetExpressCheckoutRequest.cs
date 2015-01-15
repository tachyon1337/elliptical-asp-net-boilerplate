using System.Collections.Generic;

namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    //the subset of PayPal SetExpressCheckout Request fields the provider implements
    public interface ISetExpressCheckoutRequest
    {
        string PAYMENTREQUEST_0_PAYMENTACTION { get; }
        string PAYMENTREQUEST_0_CURRENCYCODE { get; }
        string PAYMENTREQUEST_0_AMT { get; }
        string PAYMENTREQUEST_0_DESC { get; }
        string PAYMENTREQUEST_0_INVNUM { get; }
        string EMAIL { get; }
        string RETURNURL { get; }
        string CANCELURL { get; }
        List<ExpressCheckoutItem> Items { get; }
        string PAYMENTREQUEST_0_SHIPPINGAMT { get; }
        string PAYMENTREQUEST_0_TAXAMT { get; }
        string PAYMENTREQUEST_0_ITEMAMT { get; }
        string PAYMENTREQUEST_0_SHIPTONAME { get; }
        string PAYMENTREQUEST_0_SHIPTOSTREET { get; }
        string PAYMENTREQUEST_0_SHIPTOCITY { get; }
        string PAYMENTREQUEST_0_SHIPTOSTATE { get; }
        string PAYMENTREQUEST_0_SHIPTOZIP { get; }
        string PAYMENTREQUEST_0_SHIPTOCOUNTRYCODE { get; }
        string PAYMENTREQUEST_0_SHIPTOPHONENUM { get; }
    }

    //PayPal SetExpressCheckout Request Fields for a checkout item
    public interface IExpressCheckoutItem
    {
        string L_PAYMENTREQUEST_0_QTY_mIndex { get; }
        string L_PAYMENTREQUEST_0_AMT_mIndex { get; }
        string L_PAYMENTREQUEST_0_NAME_mIndex { get; }
        string L_PAYMENTREQUEST_0_NUMBER_mIndex { get; }
    }
}