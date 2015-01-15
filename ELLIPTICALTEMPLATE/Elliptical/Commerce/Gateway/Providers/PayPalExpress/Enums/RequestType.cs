using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    /// <summary>
    /// Request Types that can be sent to PayPal
    /// </summary>
    public enum RequestType
    {
        SetExpressCheckout,
        GetExpressCheckoutDetails,
        DoExpressCheckoutPayment,
    }
}